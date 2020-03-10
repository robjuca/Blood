/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using Microsoft.EntityFrameworkCore;

using Server.Models.Component;
using Server.Models.Infrastructure;
//---------------------------//

namespace Server.Context.Component
{
  public sealed class TModelContext : DbContext, IModelContext
  {
    #region Property
    #region Settings
    public DbSet<Settings> Settings
    {
      get; set;
    }
    #endregion

    #region Category
    public DbSet<CategoryRelation> CategoryRelation
    {
      get; set;
    }
    #endregion

    #region Component
    public DbSet<ComponentDescriptor> ComponentDescriptor
    {
      get; set;
    }

    public DbSet<ComponentInfo> ComponentInfo
    {
      get; set;
    }

    public DbSet<ComponentStatus> ComponentStatus
    {
      get; set;
    }

    public DbSet<ComponentRelation> ComponentRelation
    {
      get; set;
    }
    #endregion

    #region Extension
    //public DbSet<ExtensionDocument> ExtensionDocument
    //{
    //  get; set;
    //}

    public DbSet<ExtensionGeometry> ExtensionGeometry
    {
      get; set;
    }

    public DbSet<ExtensionImage> ExtensionImage
    {
      get; set;
    }

    public DbSet<ExtensionLayout> ExtensionLayout
    {
      get; set;
    }

    public DbSet<ExtensionNode> ExtensionNode
    {
      get; set;
    }

    public DbSet<ExtensionText> ExtensionText
    {
      get; set;
    }

    public DbSet<ExtensionContent> ExtensionContent
    {
      get; set;
    }
    #endregion

    public static string ConnectionString
    {
      get;
      set;
    }
    #endregion

    #region Constructor
    public TModelContext ()
    {
    }

    public TModelContext (string connectionString)
    {
      ConnectionString = connectionString;
    }
    #endregion

    #region Interface
    void IModelContext.DisposeNow ()
    {
      Dispose ();
    }
    #endregion

    #region Overrides
    protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
    {
      if (optionsBuilder != null) {
        if (!optionsBuilder.IsConfigured) {
          optionsBuilder.UseSqlServer (ConnectionString);
        }
      }
    }
    #endregion

    #region Property
    public static TModelContext CastTo (IModelContext modelContext) => (modelContext as TModelContext);
    #endregion
  };
  //---------------------------//

}  // namespace