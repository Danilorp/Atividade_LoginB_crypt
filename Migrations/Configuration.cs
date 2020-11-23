namespace Encriptacao.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Encriptacao.Context.Contexto>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled =AutomaticMigrationsEnabled;
        }

        protected override void Seed(Encriptacao.Context.Contexto context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
