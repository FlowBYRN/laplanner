using AutoMapper;
using Trainingsplanner.Postgres.Data.Models;
using Trainingsplanner.Postgres.ViewModels;

namespace Trainingsplanner.Postgres.BuisnessLogic.Mapping
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            //TrainingsModule
            CreateMap<TrainingsModule, TrainingsModuleDto>(MemberList.Destination);
            CreateMap<TrainingsModuleDto, TrainingsModule>(MemberList.Source);

            //TrainigsAppointment
            CreateMap<TrainingsAppointment, TrainingsAppointmentDto>(MemberList.Destination);
            CreateMap<TrainingsAppointmentDto, TrainingsAppointment>(MemberList.Source);

            //TrainingsExercise
            CreateMap<TrainingsExercise, TrainingsExerciseDto>(MemberList.Destination);
            CreateMap<TrainingsExerciseDto, TrainingsExercise>(MemberList.Source);

            //TrainingsGroups
            CreateMap<TrainingsGroup, TrainingsGroupDto>(MemberList.Destination);
            CreateMap<TrainingsGroupDto, TrainingsGroup>(MemberList.Source);

            //TrainingsModuleTags
            CreateMap<TrainingsModuleTag, TrainingsModuleTagDto>(MemberList.Destination);
            CreateMap<TrainingsModuleTagDto, TrainingsModuleTag>(MemberList.Source);

            //TrainingsAppointmentTrainingsModule
            CreateMap<TrainingsAppointmentTrainingsModule, TrainingsAppointmentTrainingsModuleDto>(MemberList.Destination);
            CreateMap<TrainingsAppointmentTrainingsModuleDto, TrainingsAppointmentTrainingsModule>(MemberList.Source);

            //TrainingsModuleTrainingsExercise
            CreateMap<TrainingsModuleTrainingsExercise, TrainingsModuleTrainingsExerciseDto>(MemberList.Destination);
            CreateMap<TrainingsModuleTrainingsExerciseDto, TrainingsModuleTrainingsExercise>(MemberList.Source);

            //TrainingsGroupApplicationUser
            CreateMap<TrainingsGroupApplicationUser, TrainingsGroupApplicationUserDto>(MemberList.Destination);
            CreateMap<TrainingsGroupApplicationUserDto, TrainingsGroupApplicationUser>(MemberList.Source);

            //TrainingsModuleFollow
            CreateMap<TrainingsModuleFollow, TrainingsModuleFollowDto>(MemberList.Destination);
            CreateMap<TrainingsModuleFollowDto, TrainingsModuleFollow>(MemberList.Source);
        }
    }
}