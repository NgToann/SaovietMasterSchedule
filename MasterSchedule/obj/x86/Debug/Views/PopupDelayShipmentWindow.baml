   M S B A M L   `   `   `  ����  MasterSchedule3    -MasterSchedule.Views.PopupDelayShipmentWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Popup Delay Shipment��5   	   $	��300��6&   $	��300��63   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5
   
   �� $��DELAY SHIPMENT��6   $
��Bold"�6*    �� TextAlignment$ Center=�6<   $��30��6S   
���  ��$��Center=�6p    �� Padding$ 0,6,0,0q�6�   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dgDelayShipment��5         RowHeaderWidth$ 1Q�6,   $��1��6@      AutoGenerateColumns .     CanUserAddRows .   �� AlternationCount$ 2��6�   	   AlternatingRowBackground
	 �����
   HorizontalGridLinesBrush

 �����   VerticalGridLinesBrush
 �����   
IsReadOnly .    Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Style��6?   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6%   �� *Style+6?     5      $	 PO#��6C    6%   �� *	ProductNo+6C     5      $ Assembly
Balance��6I    6%   �� *AssemblyBalance+6I     5      $ Sewing
Balance��6G    6%   �� *SewingBalance+6G     5      $ Stockfit
Balance��6H    6%   �� *OutsoleBalance+6H     5         MinWidth$ 80Q�6%   $	 CSD��6f    63   �� *
OrderCSD+ �� StringFormat$ MM/dd��6f   5      5      5      