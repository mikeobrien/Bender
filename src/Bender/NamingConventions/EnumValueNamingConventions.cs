namespace Bender.NamingConventions
{
    public class EnumValueNamingConventions
    {
        public static NamingConventions<EnumContext> Create()
        {
            return new NamingConventions<EnumContext>(DefaultConvention);
        }

        public static string DefaultConvention(EnumContext context)
        {
            return context.Value?.ToString();
        }
    }
}