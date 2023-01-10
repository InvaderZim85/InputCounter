using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using InputCounter.Common.Enums;
using InputCounter.Data;
using InputCounter.Hooks;
using InputCounter.Model;
using InputCounter.Model.Stats;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Timer = System.Timers.Timer;

namespace InputCounter.Business;

/// <summary>
/// Provides the functions for the interaction with the data
/// </summary>
internal sealed class DataManager
{
    #region Events

    /// <summary>
    /// Occurs when the user hits a key on the keyboard
    /// </summary>
    public event EventHandler? KeyboardClicked;

    /// <summary>
    /// Occurs when the user hits a button on the mouse
    /// </summary>
    public event EventHandler? MouseClicked;

    /// <summary>
    /// Occurs when the statistics were updated
    /// </summary>
    public event EventHandler? StatsUpdated;

    #endregion

    #region Hooks

    /// <summary>
    /// The hook for the keyboard
    /// </summary>
    private GlobalKeyboardHook? _keyboardHook;

    /// <summary>
    /// The hook for the mouse
    /// </summary>
    private GlobalMouseHook? _mouseHook;

    #endregion

    /// <summary>
    /// The stats timer
    /// </summary>
    private readonly Timer _statsTimer = new(TimeSpan.FromMinutes(1));

    /// <summary>
    /// Contains the keyboard key queue
    /// </summary>
    private readonly ConcurrentQueue<Key> _keyboardQueue = new();

    /// <summary>
    /// Contains the mouse action queue
    /// </summary>
    private readonly ConcurrentQueue<MouseActionType> _mouseQueue = new();

    /// <summary>
    /// Contains the list with the threads
    /// </summary>
    private readonly List<Thread> _threads = new();

    /// <summary>
    /// Contains the value which indicates if the "stop" was requested
    /// </summary>
    private bool _stopRequested;

    /// <summary>
    /// The start date for the keyboard (needed for the day switch)
    /// </summary>
    private DateTime _startDateKeyboard;

    /// <summary>
    /// The start date for the mouse (needed for the day switch)
    /// </summary>
    private DateTime _startDateMouse;

    /// <summary>
    /// Contains the hash value of the statistics
    /// </summary>
    private int _statsHashValue;

    /// <summary>
    /// Gets the click count values
    /// </summary>
    public ClickCount ClickCountValues { get; } = new();

    /// <summary>
    /// Gets the statistics
    /// </summary>
    public Statistics Statistics { get; private set; } = new();

    /// <summary>
    /// Creates a new instance of the <see cref="DataManager"/>
    /// </summary>
    public DataManager()
    {
        // Create the database and upgrade it (if needed)
        var context = new AppDbContext();
        context.Database.Migrate();

        _startDateKeyboard = DateTime.Now;
        _startDateMouse = DateTime.Now;
    }

    #region General

    /// <summary>
    /// Prepares the databases
    /// </summary>
    /// <returns>The awaitable task</returns>
    public static async Task PrepareDatabaseAsync()
    {
        await using var context = new AppDbContext();
        await context.PrepareKeyCountTableAsync();
        await context.PrepareMouseCountTableAsync();
    }

    #endregion

    #region Start / stop

    /// <summary>
    /// Starts the queue consumer and the hooks
    /// </summary>
    public void Start()
    {
        try
        {
            // Step 1: Add the consumer
            _threads.Add(new Thread(ConsumeKeyboardQueue));
            _threads.Add(new Thread(ConsumeMouseQueue));

            // Step 2: Add the hooks
            _keyboardHook = new GlobalKeyboardHook();
            _mouseHook = new GlobalMouseHook();

            // Step 2.1: Add the methods of the hooks
            _keyboardHook.KeyPressed += (_, e) =>
            {
                AddKey(e.Key);
                KeyboardClicked?.Invoke(this, e);
            };

            _mouseHook.MouseEvent += (_, e) =>
            {
                if (e.Action is not (MouseActionType.LeftButtonDown or MouseActionType.RightButtonUp))
                    return;

                AddMouseAction(e.Action);
                MouseClicked?.Invoke(this, EventArgs.Empty);
            };

            // Step 3: Add the stats timer
            _statsTimer.Elapsed += async (_, _) =>
            {
                // Disable the timer
                _statsTimer.Enabled = false;

                // Update the statistics
                await LoadStatisticsAsync();

                // Enable the timer
                _statsTimer.Enabled = true;
            };

            // Step 4: Start the consumer
            foreach (var thread in _threads)
            {
                thread.Start();
            }

            // Step 5: Start the hooks
            _keyboardHook.Start();
            _mouseHook.Start();

            // Step 6: Start the stats timer
            _statsTimer.Start();
        }
        catch (Exception ex)
        {
            Stop();

            Log.Error(ex, "An error has occurred.");
            MessageBox.Show(ex.Message, "Error");
        }
    }

    /// <summary>
    /// Stops the queue consumer and the hooks
    /// </summary>
    public void Stop()
    {
        // Step 1: Stop the statistics timer
        _statsTimer.Stop();
        _statsTimer.Dispose();

        // Step 2: Stop the hooks
        _keyboardHook?.Stop();
        _mouseHook?.Stop();

        // Step 3: Wait until the queues are empty
        while (!_keyboardQueue.IsEmpty && !_mouseQueue.IsEmpty)
        {
            Thread.Sleep(10);
        }

        // Step 4: Stop the consumer
        _stopRequested = true;

        // Step 5: Clear the thread list
        _threads.Clear();
    }

    #endregion

    #region Add methods

    /// <summary>
    /// Adds the key to the queue
    /// </summary>
    /// <param name="key">The key which was pressed</param>
    private void AddKey(Key key)
    {
        if (_startDateKeyboard.Date != DateTime.Now.Date)
        {
            ClickCountValues.PreviousKeyboard = ClickCountValues.TodayKeyboard;
            ClickCountValues.TodayKeyboard = 1;

            _startDateKeyboard = DateTime.Now;
        }
        else
        {
            ClickCountValues.TodayKeyboard++;
        }


        _keyboardQueue.Enqueue(key);
    }

    /// <summary>
    /// Adds the mouse action to the queue (only left and right click would be added)
    /// </summary>
    /// <param name="action">The message which should be added</param>
    private void AddMouseAction(MouseActionType action)
    {
        if (_startDateMouse.Date != DateTime.Now.Date)
        {
            ClickCountValues.PreviousMouseLeft = ClickCountValues.TodayMouseLeft;
            ClickCountValues.PreviousMouseRight = ClickCountValues.TodayMouseRight;

            if (action == MouseActionType.LeftButtonDown)
                ClickCountValues.TodayMouseLeft = 1;
            else if (action == MouseActionType.RightButtonUp)
                ClickCountValues.TodayMouseRight = 1;

            _startDateMouse = DateTime.Now;
        }
        else
        {
            if (action == MouseActionType.LeftButtonDown)
                ClickCountValues.TodayMouseLeft++;
            else if (action == MouseActionType.RightButtonUp)
                ClickCountValues.TodayMouseRight++;
        }

        _mouseQueue.Enqueue(action);
    }

    #endregion

    #region Consumer

    /// <summary>
    /// Consumes the keyboard queue
    /// </summary>
    private async void ConsumeKeyboardQueue()
    {
        while (!_stopRequested)
        {
            while (!_keyboardQueue.IsEmpty)
            {
                await using var context = new AppDbContext();

                while (_keyboardQueue.TryDequeue(out var key))
                {
                    await context.InsertKeyAsync(key);
                }
            }

            // Wait 50 milliseconds before the next try
            Thread.Sleep(50);
        }
    }

    /// <summary>
    /// Consumes the mouse queue
    /// </summary>
    private async void ConsumeMouseQueue()
    {
        while (!_stopRequested)
        {
            while (!_mouseQueue.IsEmpty)
            {
                await using var context = new AppDbContext();

                while (_mouseQueue.TryDequeue(out var message))
                {
                    await context.InsertMouseActionAsync(message);
                }
            }

            // Wait 50 milliseconds before the next try
            Thread.Sleep(50);
        }
    }

    #endregion

    #region Loading

    /// <summary>
    /// Loads the counts
    /// </summary>
    /// <returns>The awaitable task</returns>
    public async Task LoadCountsAsync()
    {
        await using var context = new AppDbContext();

        // Keyboard stats
        ClickCountValues.PreviousKeyboard = await context.LoadCountAsync(StatisticType.Keyboard, DateType.Yesterday);
        ClickCountValues.TodayKeyboard = await context.LoadCountAsync(StatisticType.Keyboard, DateType.Today);

        // Mouse stats
        var (previousLeft, previousRight) = await context.LoadMouseCountAsync(DateType.Yesterday);
        var (todayLeft, todayRight) = await context.LoadMouseCountAsync(DateType.Today);
        ClickCountValues.PreviousMouseLeft = previousLeft;
        ClickCountValues.PreviousMouseRight = previousRight;
        ClickCountValues.TodayMouseLeft = todayLeft;
        ClickCountValues.TodayMouseRight = todayRight;
    }

    /// <summary>
    /// Loads the statistics
    /// </summary>
    /// <returns>The awaitable task</returns>
    public async Task LoadStatisticsAsync()
    {
        await using var context = new AppDbContext();
        Statistics = await context.LoadStatisticsAsync();

        var statsHash = Statistics.GetHashCode();

        if (_statsHashValue == statsHash)
            return;

        _statsHashValue = statsHash;

        StatsUpdated?.Invoke(this, EventArgs.Empty);
    }

    #endregion
}