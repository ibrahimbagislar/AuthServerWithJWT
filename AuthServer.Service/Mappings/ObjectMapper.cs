using AutoMapper;

namespace AuthServer.Service.Mappings
{
    public static class ObjectMapper
    {
        private static readonly Lazy<IMapper> lazy = new(() =>
        {
            var config = new MapperConfiguration(opt =>
            {
                opt.AddProfiles(new List<Profile>()
                {
                    new MapperDto()
                });
            });
            return config.CreateMapper();
        });

        public static IMapper Mapper => lazy.Value;
    }
}
