using System;

namespace Infrastructure
{
    public static class AppPolicies
    {
        public const string CanCreateContent = "CanCreateContent";
        public const string CanAdministrate = "IsAdmin";
        public const string CanEditTrainingsGroup = "EditTrainingsGroup";
        public const string CanReadTrainingsGroup = "ReadTrainingsGroup";
        public const string CanEditTrainingsModule = "EditTrainingsModule";
        public const string CanEditTrainingsAppointment = "EditTrainingsAppointment";
    }
}