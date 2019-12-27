/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using rr.Library.Message;
using rr.Library.Types;

using Shared.Types;
//---------------------------//

namespace Shared.Message
{
  public class TNode : TNode<TChild>
  {
    #region Property
    public bool IsChildren
    {
      get
      {
        return (Child.Equals (TChild.None));
      }
    }

    public TModuleName ModuleName
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    TNode (TTypeInfo typeInfo)
      : base (TChild.None, typeInfo)
    {
      ModuleName = TModuleName.None;
    }
    #endregion

    #region Members
    public bool IsChild (TChild child)
    {
      return (Child.Equals (child));
    }

    public bool NotMe (TChild child, TTypeInfo typeInfo)
    {
      if (Child.Equals (child)) {
        return (TypeInfo.IsSame (typeInfo).IsFalse ());
      }

      return (Child.Equals (child).IsFalse ());
    }

    public bool IsParentToMe (TChild child)
    {
      bool res = false;

      if (IsRelationParent) {
        if (IsChildren || IsChild (child)) {
          res = true;
        }
      }

      return (res);
    }

    public bool IsSiblingToMe (TChild child, TTypeInfo typeInfo)
    {
      bool res = false;

      if (IsRelationSibling) {
        if (NotMe (child, typeInfo)) {
          res = true;
        }
      }

      return (res);
    }

    public bool IsModuleName (TModuleName moduleName)
    {
      return (ModuleName.Equals (moduleName));
    }

    public void SelectRelationModule (TChild child, TModuleName moduleName)
    {
      ModuleName = moduleName;

      SelectRelationModule (child);
    }
    #endregion

    #region Property
    public static TNode Create (TTypeInfo typeInfo) => (new TNode (typeInfo));
    #endregion
  };
  //---------------------------//

}  // namespace