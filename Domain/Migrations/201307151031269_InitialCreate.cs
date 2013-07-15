namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        CompanyID = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(nullable: false),
                        CompanyAddress = c.String(),
                        CompanyRegon = c.String(),
                        AdditionalData = c.String(),
                    })
                .PrimaryKey(t => t.CompanyID);
            
            CreateTable(
                "dbo.EquipmentTypes",
                c => new
                    {
                        EquipmentTypeID = c.Int(nullable: false, identity: true),
                        EquipmentTypeName = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.EquipmentTypeID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        cost = c.Double(nullable: false),
                        receivingDate = c.DateTime(nullable: false),
                        endDate = c.DateTime(nullable: false),
                        additionalInformation = c.String(),
                        Customer_UserID = c.Int(nullable: false),
                        EquipmentType_EquipmentTypeID = c.Int(nullable: false),
                        OrderStatus_OrderStatusID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.Users", t => t.Customer_UserID, cascadeDelete: true)
                .ForeignKey("dbo.EquipmentTypes", t => t.EquipmentType_EquipmentTypeID, cascadeDelete: true)
                .ForeignKey("dbo.OrderStatus", t => t.OrderStatus_OrderStatusID, cascadeDelete: true)
                .Index(t => t.Customer_UserID)
                .Index(t => t.EquipmentType_EquipmentTypeID)
                .Index(t => t.OrderStatus_OrderStatusID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Login = c.String(nullable: false, maxLength: 30),
                        Password = c.String(nullable: false),
                        PasswordSalt = c.String(),
                        Name = c.String(),
                        Surname = c.String(),
                        Address = c.String(),
                        Phone = c.String(),
                        additionalInformation = c.String(),
                        UserRole_UserRoleID = c.Int(),
                        Company_CompanyID = c.Int(),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.UserRoles", t => t.UserRole_UserRoleID)
                .ForeignKey("dbo.Companies", t => t.Company_CompanyID)
                .Index(t => t.UserRole_UserRoleID)
                .Index(t => t.Company_CompanyID);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserRoleID = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.UserRoleID);
            
            CreateTable(
                "dbo.OrderStatus",
                c => new
                    {
                        OrderStatusID = c.Int(nullable: false, identity: true),
                        OrderStatusName = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.OrderStatusID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "Company_CompanyID" });
            DropIndex("dbo.Users", new[] { "UserRole_UserRoleID" });
            DropIndex("dbo.Orders", new[] { "OrderStatus_OrderStatusID" });
            DropIndex("dbo.Orders", new[] { "EquipmentType_EquipmentTypeID" });
            DropIndex("dbo.Orders", new[] { "Customer_UserID" });
            DropForeignKey("dbo.Users", "Company_CompanyID", "dbo.Companies");
            DropForeignKey("dbo.Users", "UserRole_UserRoleID", "dbo.UserRoles");
            DropForeignKey("dbo.Orders", "OrderStatus_OrderStatusID", "dbo.OrderStatus");
            DropForeignKey("dbo.Orders", "EquipmentType_EquipmentTypeID", "dbo.EquipmentTypes");
            DropForeignKey("dbo.Orders", "Customer_UserID", "dbo.Users");
            DropTable("dbo.OrderStatus");
            DropTable("dbo.UserRoles");
            DropTable("dbo.Users");
            DropTable("dbo.Orders");
            DropTable("dbo.EquipmentTypes");
            DropTable("dbo.Companies");
        }
    }
}
