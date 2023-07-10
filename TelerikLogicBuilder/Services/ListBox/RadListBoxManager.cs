using ABIS.LogicBuilder.FlowBuilder.Exceptions;
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

        private bool _isUpdate;
        private int _indexSelectedForEdit = -1;//ListBox.SelectedIndex can change during edit when there are other invalid items

        public event EventHandler<EventArgs>? ListChanged;

        private bool IsUpdate
        {
            get { return _isUpdate; }
            set
            {
                _isUpdate = value;
                SetButtonEnabledStates(_isUpdate);
            }
        }

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
            ListChanged?.Invoke(this, EventArgs.Empty);
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

            listBoxHost.UpdateInputControls((T)ListBox.SelectedValue);
        }

        public void Edit()
        {
            if (ListBox.SelectedIndex == -1)
                return;

            IsUpdate = true;
            _indexSelectedForEdit = ListBox.SelectedIndex;
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
            ListChanged?.Invoke(this, EventArgs.Empty);
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
            ListChanged?.Invoke(this, EventArgs.Empty);
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

            ListChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public void ResetControls()
        {
            IsUpdate = false;
            listBoxHost.DisableControlsDuringEdit(false);
            listBoxHost.ClearInputControls();
        }

        public void RestoreEnabledControls()
        {
            SetButtonEnabledStates(IsUpdate);
        }

        public bool Update(T item)
        {
            listBoxHost.ClearMessage();
            if (item.Errors.Count > 0)
            {
                listBoxHost.SetErrorMessage(string.Join(Environment.NewLine, item.Errors.ToArray()));
                return false;
            }

            if (_indexSelectedForEdit == -1)
                throw new CriticalLogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.invalidArgumentTextFormat, "{B84A56FE-E22C-4081-89BB-27DCB8C97855}"));

            int index = _indexSelectedForEdit;
            HashSet<T> unSelectedItems = GetUnSelectedItems();
            if (unSelectedItems.Contains(item))
            {
                listBoxHost.SetErrorMessage(string.Format(CultureInfo.CurrentCulture, Strings.itemAlreadyListed, item));
                return false;
            }

            ListBox.Items.Remove(ListBox.Items[_indexSelectedForEdit]);
            ListBox.Items.Insert(index, new RadListDataItem(item.ToString(), item));

            ListBox.SelectedValue = item;

            ResetControls();
            ListChanged?.Invoke(this, EventArgs.Empty);
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
            ListBox.Disposed += ListBox_Disposed;
            IsUpdate = false;
        }

        private void RemoveEventHandlers()
        {
            ListBox.SelectedIndexChanged -= ListBox_SelectedIndexChanged;
        }

        private void SetButtonEnabledStates(bool isUpdate)
        {
            _isUpdate = isUpdate;
            bool itemSelected = ListBox.SelectedIndex != -1;
            BtnAdd.Visible = !isUpdate;
            BtnUpdate.Visible = isUpdate;

            BtnCancel.Enabled = true;
            BtnDown.Enabled = !isUpdate && itemSelected;
            BtnUp.Enabled = !isUpdate && itemSelected;
            BtnCopy.Enabled = !isUpdate && itemSelected;
            BtnEdit.Enabled = !isUpdate && itemSelected;
            BtnRemove.Enabled = !isUpdate && itemSelected;
            ListBox.Enabled = !isUpdate;
        }

        #region Event Handlers
        private void ListBox_Disposed(object? sender, EventArgs e)
        {
            RemoveEventHandlers();
        }

        private void ListBox_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            SetButtonEnabledStates(IsUpdate);
        }
        #endregion Event Handlers
    }
}
