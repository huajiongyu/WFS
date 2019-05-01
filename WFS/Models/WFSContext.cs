using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace WFS.Models
{
    public class WFSContext : DbContext
    {
        //您的上下文已配置为从您的应用程序的配置文件(App.config 或 Web.config)
        //使用“WFSContext”连接字符串。默认情况下，此连接字符串针对您的 LocalDb 实例上的
        //“WFS.Models.WFSContext”数据库。
        // 
        //如果您想要针对其他数据库和/或数据库提供程序，请在应用程序配置文件中修改“WFSContext”
        //连接字符串。
        public WFSContext()
            : base("name=WFSContext")
        {
            //在这里可以改变生成数据库的初始化策略
            //1. CreateDatabaseIfNotExists （EF的默认策略，数据库不存在,生成数据库；一旦model发生变化,抛异常，提示走数据迁移)
            //Database.SetInitializer<dbContext6>(new CreateDatabaseIfNotExists<dbContext6>());

            //2. DropCreateDatabaseIfModelChanges （一旦model发生变化,删除数据库重新生成）
            //Database.SetInitializer<WFSContent>(new DropCreateDatabaseIfModelChanges<WFSContent>());

            //3.DropCreateDatabaseAlways （数据库每次都重新生成）
            //Database.SetInitializer<dbContext6>(new DropCreateDatabaseAlways<dbContext6>());

            //4. 自定义初始化(继承上面的三种策略中任何一种，然后追加自己的业务)
            //Database.SetInitializer<dbContext6>(new MySpecialIntializer());

            //5. 禁用数据库策略（不会报错，不会丢失数据，但是改变不了数据库的结构了）
            //Database.SetInitializer<dbContext6>(null);

            //数据库迁移的初始化方式
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<WFSContext, Configuration>("WFSContext"));
        }

        //为您要在模型中包含的每种实体类型都添加 DbSet。有关配置和使用 Code First  模型
        //的详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=390109。

        /// <summary>
        /// 用户表
        /// </summary>
        public virtual DbSet<UserEntity> Users { get; set; }

        /// <summary>
        /// 申请表
        /// </summary>
        public virtual DbSet<FormEntity> Forms { get; set; }

        /// <summary>
        /// 参数表
        /// </summary>
        public virtual DbSet<MetaValues> MetaValues { get; set; }

        /// <summary>
        /// 银行列表
        /// </summary>
        public virtual DbSet<Bank> Banks { get; set; }

        /// <summary>
        /// 部门
        /// </summary>

        public virtual DbSet<Deptment> Deptments { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
    
    internal sealed class Configuration : DbMigrationsConfiguration<WFSContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;  //启用自动迁移
            AutomaticMigrationDataLossAllowed = true;   //更改数据库中结构(增加、删除列、修改列、改变列的属性、增加、删除、修改表),需要显示开启。
        }

    }
}