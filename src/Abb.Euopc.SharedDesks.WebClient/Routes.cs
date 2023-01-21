namespace Abb.Euopc.SharedDesks.WebClient;

internal static class Routes
{
    public const string Dashboard = "/";
    public const string Reservation = "reservation";

    public static class Admin
    {
        public const string Root = $"admin";

        public static class Area
        {
            private const string Root = $"{Admin.Root}/area";
            public const string Overview = $"{Root}/overview";
            public const string Add = $"{Root}/add";
            public const string Edit = $"{Root}/edit";
            public const string Delete = $"{Root}/delete";
        }

        public static class Desk
        {
            private const string Root = $"{Admin.Root}/desk";
            public const string Overview = $"{Root}/overview";
            public const string Add = $"{Root}/add";
            public const string Edit = $"{Root}/edit";
            public const string Delete = $"{Root}/delete";
        }

        public static class DeskItem
        {
            private const string Root = $"{Admin.Root}/deskitem";
            public const string Overview = $"{Root}/overview";
            public const string Add = $"{Root}/add";
            public const string Edit = $"{Root}/edit";
            public const string Delete = $"{Root}/delete";
        }

        public static class DeskItemType
        {
            private const string Root = $"{Admin.Root}/deskitemtype";
            public const string Overview = $"{Root}/overview";
            public const string Add = $"{Root}/add";
            public const string Edit = $"{Root}/edit";
            public const string Delete = $"{Root}/delete";
        }
    }
}

