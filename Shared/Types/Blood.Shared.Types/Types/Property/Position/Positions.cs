/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
//---------------------------//

namespace Shared.Types
{
  public struct Positions : System.IEquatable<Positions>
  {
    public enum Image
    {
      left,
      right,
      top,
      bottom,
      full,
      none,
    };

    #region Property
    static public string LEFT
    {
      get
      {
        return (Image.left.ToString ());
      }
    }

    static public string RIGHT
    {
      get
      {
        return (Image.right.ToString ());
      }
    }

    static public string TOP
    {
      get
      {
        return (Image.top.ToString ());
      }
    }

    static public string BOTTOM
    {
      get
      {
        return (Image.bottom.ToString ());
      }
    }

    static public string FULL
    {
      get
      {
        return (Image.full.ToString ());
      }
    }

    static public string NONE
    {
      get
      {
        return (Image.none.ToString ());
      }
    }
    #endregion

    public static readonly string [] Names = new string [] { LEFT, RIGHT, TOP, BOTTOM, FULL, NONE };

    public override bool Equals (object obj)
    {
      return (false);
    }

    public override int GetHashCode ()
    {
      return (0);
    }

    public static bool operator == (Positions left, Positions right)
    {
      return (left.Equals (right));
    }

    public static bool operator != (Positions left, Positions right)
    {
      return (!(left == right));
    }

    public bool Equals (Positions other)
    {
      return (false);
    }
  };
  //---------------------------//

}  // namespace