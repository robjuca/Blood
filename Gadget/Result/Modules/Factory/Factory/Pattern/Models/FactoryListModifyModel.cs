/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Shared.Gadget.Models.Action;
using Shared.Gadget.Models.Component;
//---------------------------//

namespace Gadget.Factory.Pattern.Models
{
  public class TFactoryListModifyModel
  {
    #region Property
    public GadgetRegistration Registration
    {
      get; 
    }

    public ObservableCollection<GadgetTest> TestItemsSource
    {
      get;
    }

    public GadgetTest Current
    {
      get
      {
        return (TestSelectedIndex.Equals (-1) ? GadgetTest.CreateDefault : TestItemsSource [TestSelectedIndex]);
      }
    }

    public string TestCount
    {
      get
      {
        return ($"[ {TestItemsSource.Count} ]");
      }
    }

    public int TestSelectedIndex
    {
      get; 
      set;
    }
    #endregion

    #region Constructor
    public TFactoryListModifyModel ()
    {
      Registration = GadgetRegistration.CreateDefault;

      TestItemsSource = new ObservableCollection<GadgetTest> ();

      TestSelectedIndex = -1;
    }
    #endregion

    #region Members
    internal void ModifyEnter (TActionComponent component)
    {
      component.ThrowNull ();

      Cleanup ();

      var gadget = component.Models.GadgetResultModel;
      gadget.RequestContent (Registration);
      gadget.RequestContent (TestItemsSource);

      if (TestItemsSource.Any ()) {
        TestSelectedIndex = 0;
      }
    }

    internal void TestChanged ()
    {
      
    }

    internal void Cleanup ()
    {
      Registration.CopyFrom (GadgetRegistration.CreateDefault);

      TestItemsSource.Clear ();

      TestSelectedIndex = -1;
    }
    #endregion

  };
  //---------------------------//

}  // namespace
