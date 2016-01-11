using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using ContactManager.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ContactManager.Pages.MasterDetail
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MasterDetailPage
    {

        private Contact _selectedContact;

        private readonly ObservableCollection<Contact> _contacts;

        public MasterDetailPage()
        {
            InitializeComponent();
            Loaded += OnLoaded;

            // Get the contacts from a Service
            // Remember to enable the NavigationCacheMode of this Page to avoid
            // load the data every time user navigates back and forward.    
            _contacts = Contact.GetContacts(25);
            if (_contacts.Count > 0)
            {
                MasterListView.ItemsSource = _contacts;
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Details view can remove an item from the list.
            if (e.Parameter as string == "Delete")
            {
                DeleteItem(null, null);
            }
            base.OnNavigatedTo(e);
        }
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (_selectedContact == null && _contacts.Count > 0)
            {
                _selectedContact = _contacts[0];
                MasterListView.SelectedIndex = 0;
            }
            // If the app starts in narrow mode - showing only the Master listView - 
            // it is necessary to set the commands and the selection mode.
            if (PageSizeStatesGroup.CurrentState == NarrowState)
            {
                VisualStateManager.GoToState(this, MasterState.Name, true);
            }
            else if (PageSizeStatesGroup.CurrentState == WideState)
            {
                // In this case, the app starts is wide mode, Master/Details view, 
                // so it is necessary to set the commands and the selection mode.
                VisualStateManager.GoToState(this, MasterDetailsState.Name, true);
                MasterListView.SelectionMode = ListViewSelectionMode.Extended;
                MasterListView.SelectedItem = _selectedContact;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
        private void OnCurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            bool isNarrow = e.NewState == NarrowState;
            if (isNarrow)
            {
                Frame.Navigate(typeof(DetailPage), _selectedContact, new SuppressNavigationTransitionInfo());
            }
            else
            {
                VisualStateManager.GoToState(this, MasterDetailsState.Name, true);
                MasterListView.SelectionMode = ListViewSelectionMode.Extended;
                MasterListView.SelectedItem = _selectedContact;
            }

            EntranceNavigationTransitionInfo.SetIsTargetElement(MasterListView, isNarrow);
            if (DetailContentPresenter != null)
            {
                EntranceNavigationTransitionInfo.SetIsTargetElement(DetailContentPresenter, !isNarrow);
            }
        }
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PageSizeStatesGroup.CurrentState == WideState)
            {
                if (MasterListView.SelectedItems.Count == 1)
                {
                    _selectedContact = MasterListView.SelectedItem as Contact;
                    EnableContentTransitions();
                }
                // Entering in Extended selection
                else if (MasterListView.SelectedItems.Count > 1
                     && MasterDetailsStatesGroup.CurrentState == MasterDetailsState)
                {
                    VisualStateManager.GoToState(this, ExtendedSelectionState.Name, true);
                }
            }
            // Exiting Extended selection
            if (MasterDetailsStatesGroup.CurrentState == ExtendedSelectionState &&
                MasterListView.SelectedItems.Count == 1)
            {
                VisualStateManager.GoToState(this, MasterDetailsState.Name, true);
            }
        }
        // ItemClick event only happens when user is a Master state. In this state, 
        // selection mode is none and click event navigates to the details view.
        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            // The clicked item it is the new selected contact
            _selectedContact = e.ClickedItem as Contact;
            if (PageSizeStatesGroup.CurrentState == NarrowState)
            {
                // Go to the details page and display the item 
                Frame.Navigate(typeof(DetailPage), _selectedContact, new DrillInNavigationTransitionInfo());
            }
            //else
            {
                // Play a refresh animation when the user switches detail items.
                //EnableContentTransitions();
            }
        }
        private void EnableContentTransitions()
        {
            DetailContentPresenter.ContentTransitions.Clear();
            DetailContentPresenter.ContentTransitions.Add(new EntranceThemeTransition());
        }
        #region Commands
        private void AddItem(object sender, RoutedEventArgs e)
        {
            Contact c = Contact.GetNewContact();
            _contacts.Add(c);
            MasterListView.ScrollIntoView(c);

            // Select this item in case that the list is empty
            if (MasterListView.SelectedIndex == -1)
            {
                MasterListView.SelectedIndex = 0;
                _selectedContact = MasterListView.SelectedItem as Contact;
                // Details view is collapsed, in case there is not items.
                // You should show it just in case. 
                DetailContentPresenter.Visibility = Visibility.Visible;
            }
        }
        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            if (_selectedContact != null)
            {
                _contacts.Remove(_selectedContact);

                if (MasterListView.Items?.Count > 0)
                {
                    MasterListView.SelectedIndex = 0;
                    _selectedContact = MasterListView.SelectedItem as Contact;
                }
                else
                {
                    // Details view is collapsed, in case there is not items.
                    DetailContentPresenter.Visibility = Visibility.Collapsed;
                    _selectedContact = null;
                }
            }
        }
        private void DeleteItems(object sender, RoutedEventArgs e)
        {
            if (MasterListView.SelectedIndex != -1)
            {
                List<Contact> selectedItems = MasterListView.SelectedItems.Cast<Contact>().ToList();
                foreach (Contact contact in selectedItems)
                {
                    _contacts.Remove(contact);
                }
                if (MasterListView.Items?.Count > 0)
                {
                    MasterListView.SelectedIndex = 0;
                    _selectedContact = MasterListView.SelectedItem as Contact;
                }
                else
                {
                    DetailContentPresenter.Visibility = Visibility.Collapsed;
                }
            }
        }
        private void SelectItems(object sender, RoutedEventArgs e)
        {
            if (MasterListView.Items?.Count > 0)
            {
                VisualStateManager.GoToState(this, MultipleSelectionState.Name, true);
            }
        }
        private void CancelSelection(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this,
                PageSizeStatesGroup.CurrentState == NarrowState ? MasterState.Name : MasterDetailsState.Name, true);
        }

        private void ShowSliptView(object sender, RoutedEventArgs e)
        {
            // Clearing the cache
            int cacheSize = ((Frame)Parent).CacheSize;
            ((Frame)Parent).CacheSize = 0;
            ((Frame)Parent).CacheSize = cacheSize;

            MySamplesPane.SamplesSplitView.IsPaneOpen = !MySamplesPane.SamplesSplitView.IsPaneOpen;
        }
        #endregion
    }
}
