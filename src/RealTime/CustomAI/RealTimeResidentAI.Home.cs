﻿// <copyright file="RealTimeResidentAI.Home.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

namespace RealTime.CustomAI
{
    using RealTime.Tools;
    using static Constants;

    internal sealed partial class RealTimeResidentAI<TAI, TCitizen>
    {
        private void ProcessCitizenAtHome(TAI instance, uint citizenId, ref TCitizen citizen)
        {
            if (CitizenProxy.GetHomeBuilding(ref citizen) == 0)
            {
                Log.Debug($"WARNING: {GetCitizenDesc(citizenId, ref citizen)} is in corrupt state: at home with no home building. Releasing the poor citizen.");
                CitizenMgr.ReleaseCitizen(citizenId);
                return;
            }

            ushort vehicle = CitizenProxy.GetVehicle(ref citizen);
            if (vehicle != 0)
            {
                Log.Debug(TimeInfo.Now, $"WARNING: {GetCitizenDesc(citizenId, ref citizen)} is at home but vehicle = {vehicle}");
                return;
            }

            if (CitizenGoesWorking(instance, citizenId, ref citizen))
            {
                return;
            }

            if (IsBusyAtHomeInTheMorning(CitizenProxy.GetAge(ref citizen)))
            {
                return;
            }

            if (CitizenGoesShopping(instance, citizenId, ref citizen) || CitizenGoesToEvent(instance, citizenId, ref citizen))
            {
                return;
            }

            CitizenGoesRelaxing(instance, citizenId, ref citizen);
        }

        private bool IsBusyAtHomeInTheMorning(Citizen.AgeGroup citizenAge)
        {
            float currentHour = TimeInfo.CurrentHour;
            float offset = IsWeekend ? 2 : 0;
            switch (citizenAge)
            {
                case Citizen.AgeGroup.Child:
                    return IsBusyAtHomeInTheMorning(currentHour, 8 + offset);

                case Citizen.AgeGroup.Teen:
                case Citizen.AgeGroup.Young:
                    return IsBusyAtHomeInTheMorning(currentHour, 9 + offset);

                case Citizen.AgeGroup.Adult:
                    return IsBusyAtHomeInTheMorning(currentHour, 8 + (offset / 2f));

                case Citizen.AgeGroup.Senior:
                    return IsBusyAtHomeInTheMorning(currentHour, 7);

                default:
                    return true;
            }
        }

        // TODO: make this method to a part of time simulation (no need to calculate for each citizen)
        private bool IsBusyAtHomeInTheMorning(float currentHour, float latestHour)
        {
            if (currentHour >= latestHour || currentHour < Config.WakeupHour)
            {
                return false;
            }

            float wakeupHour = Config.WakeupHour;
            float dx = latestHour - wakeupHour;
            float x = currentHour - wakeupHour;

            // A cubic probability curve from the earliest wake up hour (0%) to latest hour (100%)
            uint chance = (uint)((100f / dx * x) - ((dx - x) * (dx - x) * x));
            return !Random.ShouldOccur(chance);
        }
    }
}
