using AutoMapper;
using TrainingsPlanner.Infrastructure.Models;
using TrainingsPlanner.ViewModels;

namespace TrainingsPlanner.BuisnessLogic.Mapping
{
    public static class MapperExtensions
    {
        internal static IMapper Mapper { get; }
        
        static MapperExtensions()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>())
                .CreateMapper();
        }
        
        #region TrainingsModule

        //Entity -> DTO
        public static TrainingsModuleDto ToViewModel(this TrainingsModule source) => Mapper.Map<TrainingsModuleDto>(source);

        //DTO -> Entity
        public static TrainingsModule ToEntity(this TrainingsModuleDto source) => Mapper.Map<TrainingsModule>(source);
        #endregion

        #region TrainingsAppointment
        // Entity -> DTO
        public static TrainingsAppointmentDto ToViewModel(this TrainingsAppointment source) => Mapper.Map<TrainingsAppointmentDto>(source);

        //DTO -> Entity
        public static TrainingsAppointment ToEntity(this TrainingsAppointmentDto source) => Mapper.Map<TrainingsAppointment>(source);
        #endregion

        #region TrainingsExercise

        //Entity -> DTO
        public static TrainingsExerciseDto ToViewModel(this TrainingsExercise source) => Mapper.Map<TrainingsExerciseDto>(source);
        //DTO -> Entity
        public static TrainingsExercise ToEntity(this TrainingsExerciseDto source) => Mapper.Map<TrainingsExercise>(source);
        #endregion
        
        #region TrainingsGroups
        // Entity -> DTO
        public static TrainingsGroupDto ToViewModel(this TrainingsGroup source) => Mapper.Map<TrainingsGroupDto>(source);

        //DTO -> Entity
        public static TrainingsGroup ToEntity(this TrainingsGroupDto source) => Mapper.Map<TrainingsGroup>(source);
        #endregion

        #region TrainingsModuleTag
        // Entity -> DTO
        public static TrainingsModuleTagDto ToViewModel(this TrainingsModuleTag source) => Mapper.Map<TrainingsModuleTagDto>(source);

        //DTO -> Entity
        public static TrainingsModuleTag ToEntity(this TrainingsModuleTagDto source) => Mapper.Map<TrainingsModuleTag>(source);
        #endregion

        #region TrainingsAppointmentTrainingsModule
        // Entity -> DTO
        public static TrainingsAppointmentTrainingsModuleDto ToViewModel(this TrainingsAppointmentTrainingsModule source) => Mapper.Map<TrainingsAppointmentTrainingsModuleDto>(source);

        //DTO -> Entity
        public static TrainingsAppointmentTrainingsModule ToEntity(this TrainingsAppointmentTrainingsModuleDto source) => Mapper.Map<TrainingsAppointmentTrainingsModule>(source);
        #endregion

        #region TrainingsModuleTrainingsExercise
        // Entity -> DTO
        public static TrainingsModuleTrainingsExerciseDto ToViewModel(this TrainingsModuleTrainingsExercise source) => Mapper.Map<TrainingsModuleTrainingsExerciseDto>(source);

        //DTO -> Entity
        public static TrainingsModuleTrainingsExercise ToEntity(this TrainingsModuleTrainingsExerciseDto source) => Mapper.Map<TrainingsModuleTrainingsExercise>(source);
        #endregion
        
        #region TrainingsGroupApplicationUser
        // Entity -> DTO
        public static TrainingsGroupApplicationUserDto ToViewModel(this TrainingsGroupApplicationUser source) => Mapper.Map<TrainingsGroupApplicationUserDto>(source);

        //DTO -> Entity
        public static TrainingsGroupApplicationUser ToEntity(this TrainingsGroupApplicationUserDto source) => Mapper.Map<TrainingsGroupApplicationUser>(source);
        #endregion
    }
}