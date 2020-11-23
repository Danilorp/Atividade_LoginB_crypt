namespace Encriptacao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsuarioModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UsuarioModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Senha = c.String(nullable: false, maxLength: 255),
                        Nivel = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UsuarioModels");
        }
    }
}
