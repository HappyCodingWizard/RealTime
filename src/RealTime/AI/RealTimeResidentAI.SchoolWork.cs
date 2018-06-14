﻿// <copyright file="RealTimeResidentAI.SchoolWork.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

namespace RealTime.AI
{
    using RealTime.Tools;

    internal static partial class RealTimeResidentAI
    {
        private static void ProcessCitizenAtSchoolOrWork(ResidentAI instance, References refs, uint citizenId, ref Citizen citizen)
        {
            if (citizen.m_workBuilding == 0)
            {
                Log.Debug($"WARNING: {CitizenInfo(citizenId, ref citizen)} is in corrupt state: at school/work with no work building. Teleporting home.");
                citizen.CurrentLocation = Citizen.Location.Home;
                return;
            }

            if (Logic.ShouldGoToLunch(ref citizen))
            {
                ushort lunchPlace = FindLocalCommercialBuilding(instance, refs, citizenId, ref citizen, LocalSearchDistance);
                if (lunchPlace != 0)
                {
                    Log.Debug(refs.SimMgr.m_currentGameTime, $"{CitizenInfo(citizenId, ref citizen)} is heading out to eat for lunch at {lunchPlace}");
                }
                else
                {
                    Log.Debug(refs.SimMgr.m_currentGameTime, $"{CitizenInfo(citizenId, ref citizen)} wanted to head out for lunch, but there were no buildings close enough");
                }

                return;
            }

            if (!Logic.ShouldReturnFromSchoolOrWork(ref citizen))
            {
                return;
            }

            Log.Debug(refs.SimMgr.m_currentGameTime, $"{CitizenInfo(citizenId, ref citizen)} leaves their workplace");

            if (!CitizenGoesShopping(instance, refs, citizenId, ref citizen) && !CitizenGoesRelaxing(instance, refs, citizenId, ref citizen))
            {
                instance.StartMoving(citizenId, ref citizen, citizen.m_workBuilding, citizen.m_homeBuilding);
            }
        }

        private static bool CitizenGoesWorking(ResidentAI instance, References refs, uint citizenId, ref Citizen citizen)
        {
            if (!Logic.ShouldGoToSchoolOrWork(ref citizen, refs.BuildingMgr))
            {
                return false;
            }

            Log.Debug(refs.SimMgr.m_currentGameTime, $"{CitizenInfo(citizenId, ref citizen)} is going from {citizen.GetBuildingByLocation()} to school/work {citizen.m_workBuilding}");

            if (!instance.StartMoving(citizenId, ref citizen, citizen.m_homeBuilding, citizen.m_workBuilding))
            {
                Log.Debug($"{CitizenInfo(citizenId, ref citizen)} has no instance, teleporting to school/work");
                citizen.CurrentLocation = Citizen.Location.Work;
            }

            return true;
        }
    }
}
