/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

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

    public bool SelectorContentTestEnabled
    {
      get; 
      set;
    }

    public bool SelectorContentTargetEnabled
    {
      get;
      set;
    }

    public bool SelectorContentTestChecked
    {
      get; 
      set;
    }

    public bool SelectorContentTargetChecked
    {
      get;
      set;
    }

    public int SlideIndex
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
      SlideIndex = -1;

      SelectorContentTestEnabled = false;
      SelectorContentTestChecked = false;

      SelectorContentTargetEnabled = false;
      SelectorContentTargetChecked = false;

      m_FullCollection = new Collection<GadgetTest> ();
    }
    #endregion

    #region Members
    internal void ModifyEnter (TActionComponent component)
    {
      component.ThrowNull ();

      Cleanup ();

      var gadget = component.Models.GadgetResultModel;
      gadget.RequestContent (Registration);
      gadget.RequestContent (m_FullCollection);

      foreach (var item in m_FullCollection) {
        if (item.IsContentTest) {
          SelectorContentTestEnabled = true;
        }

        if (item.IsContentTarget) {
          SelectorContentTargetEnabled = true;
        }
      }

      if (SelectorContentTestEnabled && SelectorContentTargetEnabled) {
        SlideIndex = 0;
        SelectorContentTestChecked = true;
      }

      else {
        if (SelectorContentTestEnabled) {
          SlideIndex = 0;
          SelectorContentTestChecked = true;
        }

        if (SelectorContentTargetEnabled) {
          SlideIndex = 1;
          SelectorContentTargetChecked = true;
        }
      }
    }

    internal void SelectorContentTestIsChecked ()
    {
      SlideIndex = 0;

      TestItemsSource.Clear ();

      foreach (var item in m_FullCollection) {
        if (item.IsContentTest) {
          TestItemsSource.Add (item);
        }
      }

      if (TestItemsSource.Any ()) {
        TestSelectedIndex = 0;
      }
    }

    internal void SelectorContentTargetIsChecked ()
    {
      SlideIndex = 1;

      TestItemsSource.Clear ();

      foreach (var item in m_FullCollection) {
        if (item.IsContentTarget) {
          TestItemsSource.Add (item);
        }
      }

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
      SlideIndex = -1;

      SelectorContentTestEnabled = false;
      SelectorContentTestChecked = false;

      SelectorContentTargetEnabled = false;
      SelectorContentTargetChecked = false;

      m_FullCollection.Clear ();
    }
    #endregion

    #region Fields
    readonly Collection<GadgetTest>                             m_FullCollection;
    #endregion
  };
  //---------------------------//

}  // namespace
