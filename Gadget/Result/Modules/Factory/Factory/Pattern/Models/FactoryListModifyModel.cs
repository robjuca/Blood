﻿/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
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

    #region content test
    public ObservableCollection<GadgetTest> ContentTestItemsSource
    {
      get;
    }

    public string ContentTestCount
    {
      get
      {
        return ($"[ {ContentTestItemsSource.Count} ]");
      }
    }

    public int ContentTestSelectedIndex
    {
      get;
      set;
    }

    public GadgetTest ContentTestTargetCurrent
    {
      get;
    }
    #endregion

    #region content target
    public ObservableCollection<GadgetTest> ContentTargetItemsSource
    {
      get;
    }

    public string ContentTargetCount
    {
      get
      {
        return ($"[ {ContentTargetItemsSource.Count} ]");
      }
    }

    public int ContentTargetSelectedIndex
    {
      get;
      set;
    }

    public GadgetTest ContentTargetCurrent
    {
      get
      {
        return (ContentTargetSelectedIndex.Equals (-1) ? GadgetTest.CreateDefault : ContentTargetItemsSource [ContentTargetSelectedIndex]);
      }
    }
    #endregion

    #region selector
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
    #endregion

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

      ContentTestItemsSource = new ObservableCollection<GadgetTest> ();
      ContentTestSelectedIndex = -1;
      ContentTestTargetCurrent = GadgetTest.CreateDefault;

      ContentTargetItemsSource = new ObservableCollection<GadgetTest> ();
      ContentTargetSelectedIndex = -1;

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

      if (m_SelectorContentTestDone.IsFalse ()) {
        m_SelectorContentTestDone = true;

        ContentTestItemsSource.Clear ();

        foreach (var item in m_FullCollection) {
          if (item.IsContentTest) {
            ContentTestItemsSource.Add (item);
          }
        }

        if (ContentTestItemsSource.Any ()) {
          ContentTestSelectedIndex = 0;

          var gadget = ContentTestItemsSource [0];

          if (gadget.HasContent) {
            if (gadget.IsContentTest) {
              var list = new Collection<GadgetTest> ();
              gadget.RequestContent (list);

              if (list.Any ()) {
                ContentTestTargetChanged (list [0]);
              }
            }
          }
        }
      }
    }

    internal void SelectorContentTargetIsChecked ()
    {
      SlideIndex = 1;

      if (m_SelectorContentTargetDone.IsFalse ()) {
        m_SelectorContentTargetDone = true;

        ContentTargetItemsSource.Clear ();

        foreach (var item in m_FullCollection) {
          if (item.IsContentTarget) {
            ContentTargetItemsSource.Add (item);
          }
        }

        if (ContentTargetItemsSource.Any ()) {
          ContentTargetSelectedIndex = 0;
        }
      }
    }

    internal void ContentTestTargetChanged (GadgetTest gadget)
    {
      // content Test Target
      ContentTestTargetCurrent.CopyFrom (gadget);
    }

    internal void ContentTargetChanged (GadgetTest gadget)
    {
      // content target
    }

    internal void Cleanup ()
    {
      Registration.CopyFrom (GadgetRegistration.CreateDefault);

      ContentTestItemsSource.Clear ();
      ContentTestSelectedIndex = -1;
      ContentTestTargetCurrent.CopyFrom (GadgetTest.CreateDefault);

      ContentTargetItemsSource.Clear ();
      ContentTargetSelectedIndex = -1;

      SlideIndex = -1;

      SelectorContentTestEnabled = false;
      SelectorContentTestChecked = false;

      SelectorContentTargetEnabled = false;
      SelectorContentTargetChecked = false;

      m_FullCollection.Clear ();

      m_SelectorContentTestDone = false;
      m_SelectorContentTargetDone = false;
    }
    #endregion

    #region Fields
    readonly Collection<GadgetTest>                             m_FullCollection;
    bool                                                        m_SelectorContentTestDone;
    bool                                                        m_SelectorContentTargetDone;
    #endregion
  };
  //---------------------------//

}  // namespace
