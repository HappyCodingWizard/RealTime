﻿// <copyright file="RealTimeEventBase.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

namespace RealTime.Events
{
    using System;

    internal abstract class RealTimeEventBase : IRealTimeEvent
    {
        public DateTime StartTime { get; private set; }

        public DateTime EndTime => StartTime.AddHours(GetDuration());

        public ushort BuildingId { get; private set; }

        public string BuildingName { get; private set; }

        public virtual void Attend()
        {
        }

        public virtual bool CanAttend()
        {
            return true;
        }

        public void Configure(ushort buildingId, string buildingName, DateTime startTime)
        {
            BuildingId = buildingId;
            BuildingName = buildingName ?? string.Empty;
            StartTime = startTime;
        }

        protected abstract float GetDuration();
    }
}
