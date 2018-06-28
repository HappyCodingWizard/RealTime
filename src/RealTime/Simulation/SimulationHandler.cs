﻿// <copyright file="SimulationHandler.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

namespace RealTime.Simulation
{
    using System;
    using ICities;
    using RealTime.CustomAI;
    using RealTime.Events;

    /// <summary>
    /// A central simulation handler that dispatches the simulation frame processing
    /// to the custom logic class instances.
    /// </summary>
    public sealed class SimulationHandler : ThreadingExtensionBase
    {
        private DateTime lastHandledDate;

        /// <summary>
        /// Occurs when a new day in the game begins.
        /// </summary>
        internal static event EventHandler NewDay;

        /// <summary>
        /// Gets or sets the custom event manager simulation class instance.
        /// </summary>
        internal static RealTimeEventManager EventManager { get; set; }

        /// <summary>
        /// Gets or sets the day time simulation class instance.
        /// </summary>
        internal static DayTimeSimulation DayTimeSimulation { get; set; }

        /// <summary>
        /// Gets or sets the custom commercial building simulation class instance.
        /// </summary>
        internal static RealTimeCommercialBuildingAI CommercialAI { get; set; }

        /// <summary>
        /// Called after each game simulation tick. A tick contains multiple frames.
        /// Performs the dispatching for this simulation phase.
        /// </summary>
        public override void OnAfterSimulationTick()
        {
            EventManager?.ProcessEvents();

            DateTime currentDate = SimulationManager.instance.m_currentGameTime.Date;
            if (currentDate != lastHandledDate)
            {
                lastHandledDate = currentDate;
                DayTimeSimulation?.Process(currentDate);
                OnNewDay(this);
            }
        }

        /// <summary>
        /// Called after each game simulation frame. Performs the dispatching for this simulation phase.
        /// </summary>
        public override void OnAfterSimulationFrame()
        {
            CommercialAI?.Process(SimulationManager.instance.m_currentFrameIndex);
        }

        private static void OnNewDay(SimulationHandler sender)
        {
            NewDay?.Invoke(sender, EventArgs.Empty);
        }
    }
}
