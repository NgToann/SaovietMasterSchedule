   M S B A M L   `   `   `  ����  MasterSchedule7    1MasterSchedule.Views.PopupOSWHDeliveryEarlyWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Popup Delivery Early��5   	   $	��300��6&   $	��300��63    � WindowStyle$ SingleBorderWindow=�6?   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5
   
   �� $#��PO BELOW WERE DELIVERED EARLY��6   $��30��69   
��� ���$
��Bold"�6Y    �� TextAlignment$ Center=�6k    �� Padding$ 0,6,0,0q�6�   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dgDeliveryEarly��5      $��1��6,      RowHeaderWidth$ 1Q�69      AutoGenerateColumns .     CanUserAddRows .  	 �� AlternationCount$	 2��6   
   AlternatingRowBackground

 �����   HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����   
IsReadOnly .    Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Style��6?   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6%   �� *Style+6?     5      $	 PO#��6C    6%   �� *	ProductNo+6C     5      $ Qty
Delivered��6J    6%   �� *QuantityDelivery+6J     5      $ Delivery
EFD��6_    6%   �� *DeliveryEFDDate+ �� StringFormat$ MM/dd��6_     5      $ Sewing
Start Date��6_    6%   �� *SewingStartDate+$ MM/dd��6_     5      $ Supplier��6B    6%   �� *
Supplier+6B     5      $ OS Code��6E    6%   �� *OutsoleCode+6E   5      5      5      