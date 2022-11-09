using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.ListBox
{
    internal class RadListBoxManager<T> : IRadListBoxManager<T> where T : IListBoxManageable
    {
        private readonly IListBoxHost<T> listBoxHost;

        public RadListBoxManager(IListBoxHost<T> listBoxHost)
        {
            this.listBoxHost = listBoxHost;
            Initialize();
        }

        private RadButton BtnAdd => this.listBoxHost.BtnAdd;
        private RadButton BtnUpdate => this.listBoxHost.BtnUpdate;
        private RadButton BtnCancel => this.listBoxHost.BtnCancel;
        private RadButton BtnCopy => this.listBoxHost.BtnCopy;
        private RadButton BtnEdit => this.listBoxHost.BtnEdit;
        private RadButton BtnRemove => this.listBoxHost.BtnRemove;
        private RadButton BtnUp => this.listBoxHost.BtnUp;
        private RadButton BtnDown => this.listBoxHost.BtnDown;
        private RadListControl ListBox => this.listBoxHost.ListBox;

        public bool Add(T item)
        {
            listBoxHost.ClearMessage();
            if (item.Errors.Count > 0)
            {
                listBoxHost.SetErrorMessage(string.Join(Environment.NewLine, item.Errors.ToArray()));
                return false;
            }

            HashSet<T> items = ListBox.Items.Select(i => (T)i.Value).ToHashSet();
            if (items.Contains(item))
            {
                listBoxHost.SetErrorMessage(string.Format(CultureInfo.CurrentCulture, Strings.itemAlreadyListed, item));
                return false;
            }

            ListBox.Items.Add(new RadListDataItem(item.ToString(), item));
            ListBox.SelectedValue = item;
            listBoxHost.ClearInputControls();
            return true;
        }

        public void Cancel()
        {
            ResetControls();
        }

        public void Copy()
        {
            if (ListBox.SelectedIndex == -1)
                return;

            SetButtonEnabledStates(false);
            listBoxHost.DisableControlsDuringEdit(false);
            listBoxHost.UpdateInputControls((T)ListBox.SelectedValue);
        }

        public void Edit()
        {
            if (ListBox.SelectedIndex == -1)
                return;

            SetButtonEnabledStates(true);
            listBoxHost.DisableControlsDuringEdit(true);
            listBoxHost.UpdateInputControls((T)ListBox.SelectedValue);
        }

        public void MoveDown()
        {
            if (ListBox.SelectedIndex == -1)
                return;

            if (ListBox.SelectedIndex == ListBox.Items.Count - 1)
                return;

            int newIndex = ListBox.SelectedIndex + 1;
            RadListDataItem moveItem = ListBox.SelectedItem;
            ListBox.Items.RemoveAt(ListBox.SelectedIndex);
            ListBox.Items.Insert(newIndex, moveItem);
            ListBox.SelectedItem = moveItem;
        }

        public void MoveUp()
        {
            if (ListBox.SelectedIndex == -1)
                return;

            if (ListBox.SelectedIndex == 0)
                return;

            int newIndex = ListBox.SelectedIndex - 1;
            RadListDataItem moveItem = ListBox.SelectedItem;
            ListBox.Items.RemoveAt(ListBox.SelectedIndex);
            ListBox.Items.Insert(newIndex, moveItem);
            ListBox.SelectedItem = moveItem;
        }

        public bool Remove()
        {
            int index = listBoxHost.ListBox.SelectedIndex;
            if (index == -1)
                return false;

            listBoxHost.ListBox.Items.Remove(listBoxHost.ListBox.SelectedItem);

            if (index > 0)
                listBoxHost.ListBox.SelectedIndex = index - 1;
            else if (index == 0 && listBoxHost.ListBox.Items.Count > 0)
                listBoxHost.ListBox.SelectedIndex = 0;

            return true;
        }

        public void ResetControls()
        {
            SetButtonEnabledStates(false);
            listBoxHost.DisableControlsDuringEdit(false);
            listBoxHost.ClearInputControls();
        }

        public bool Update(T item)
        {
            listBoxHost.ClearMessage();
            if (item.Errors.Count > 0)
            {
                listBoxHost.SetErrorMessage(string.Join(Environment.NewLine, item.Errors.ToArray()));
                return false;
            }

            int index = ListBox.SelectedIndex;
            HashSet<T> unSelectedItems = GetUnSelectedItems();
            if (unSelectedItems.Contains(item))
            {
                listBoxHost.SetErrorMessage(string.Format(CultureInfo.CurrentCulture, Strings.itemAlreadyListed, item));
                return false;
            }

            ListBox.Items.Remove(ListBox.SelectedItem);
            ListBox.Items.Insert(index, new RadListDataItem(item.ToString(), item));

            ListBox.SelectedValue = item;

            ResetControls();
            return true;

            HashSet<T> GetUnSelectedItems()
            {
                List<T> list = new();
                for (int i = 0; i < ListBox.Items.Count; i++)
                {
                    if (i != index)
                        list.Add((T)ListBox.Items[i].Value);
                }

                return list.ToHashSet();
            }
        }

        private void Initialize()
        {
            ListBox.SelectedIndexChanged += ListBox_SelectedIndexChanged;
        }

        private void SetButtonEnabledStates(bool isUpdate)
        {
            BtnAdd.Enabled = !isUpdate;
            BtnUpdate.Enabled = isUpdate;
            BtnCancel.Enabled = isUpdate;

            BtnDown.Enabled = !isUpdate;
            BtnUp.Enabled = !isUpdate;
            BtnCopy.Enabled = !isUpdate;
            BtnEdit.Enabled = !isUpdate;
            BtnRemove.Enabled = !isUpdate;
            ListBox.Enabled = !isUpdate;
        }

        #region Event Handlers
        private void ListBox_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
        }
        #endregion Event Handlers
    }
}
