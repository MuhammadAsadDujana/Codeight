﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BOL.dbContext
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CodEightEntities : DbContext
    {
        public CodEightEntities()
            : base("name=CodEightEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<tbl_ForgotPasswordLinks> tbl_ForgotPasswordLinks { get; set; }
        public virtual DbSet<tbl_LiveBroadCast> tbl_LiveBroadCast { get; set; }
        public virtual DbSet<tbl_Logs> tbl_Logs { get; set; }
        public virtual DbSet<tbl_Notifications> tbl_Notifications { get; set; }
        public virtual DbSet<tbl_Users> tbl_Users { get; set; }
        public virtual DbSet<tbl_VideoCategories> tbl_VideoCategories { get; set; }
        public virtual DbSet<tbl_Countries> tbl_Countries { get; set; }
        public virtual DbSet<tbl_Cities> tbl_Cities { get; set; }
        public virtual DbSet<tbl_States> tbl_States { get; set; }
        public virtual DbSet<tbl_Room> tbl_Room { get; set; }
        public virtual DbSet<tbl_Markers> tbl_Markers { get; set; }
        public virtual DbSet<tbl_NearByUser> tbl_NearByUser { get; set; }
        public virtual DbSet<tbl_ManageRadius> tbl_ManageRadius { get; set; }
    }
}
