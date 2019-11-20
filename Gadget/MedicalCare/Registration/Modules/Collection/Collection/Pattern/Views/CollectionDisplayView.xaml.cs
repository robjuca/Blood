/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
//---------------------------//

namespace Gadget.Collection.Pattern.Views
{
  public partial class TCollectionDisplayView : rr.Library.Infrastructure.ViewChildBase
  {
    #region Constructor
    public TCollectionDisplayView ()
    {
      InitializeComponent ();
    }
    #endregion

    private void Calendar_DisplayDateChanged (object sender, System.Windows.Controls.CalendarDateChangedEventArgs e)
    {

    }
  };
  //---------------------------//

}  // namespace