   M S B A M L   `   `   `  ����   MasterSchedule3    -MasterSchedule.Views.OutsoleWHInventoryWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$,  &Master Schedule - Outsole WH Inventory��$	��360��$	��640�� � WindowStartupLocation$ CenterScreen=�.���  � RowDefinitions �� $��* ��� $
��Auto �.+�&   System.Windows.Controls.DataGrid  -     Name$ dgInventory��   RowHeaderWidth$ 5Q�   
IsReadOnly .    AutoGenerateColumns .   �� AlternationCount$ 2��   AlternatingRowBackground
 �����	   HorizontalGridLinesBrush
	 �����
   VerticalGridLinesBrush

 �����   CanUserReorderColumns .     Columns 0  *System.Windows.Controls.DataGridTextColumn  -   Column1��,  &System.Windows.Controls.DataGridColumn   Header$ O/S Code��   
FontWeight$
 Bold"�   
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *OutsoleCode+  -   Column2��$ 	Total Qty�� .  �� *
Quantity+  -   Column3��$ Matching�� .  �� *
Matching+� $��1�� � ColumnDefinitions �� ����  �� ElementName$ dgInventory�� �� Path$ BorderThickness.Left��� ���� $ dgInventory��$ RowHeaderWidth��� ���� $ Column1��$ ActualWidth��� ���� $ Column2��$ ActualWidth��� ���� $ Column3��$ ActualWidth�.+��� $��2��$��1,0,1,1q�
���   �._��� $��1,0,0,0q�$��TOTAL��$
��Bold"��� $��3��$��0,0,1,1q�
���   �._��� -    ��Name$ lblTotalQTy��$��1,0,0,0q�$��0��$
��Bold"��� $��4��$��0,0,1,1q�
���   �._��� -   $ lblMatching��$��1,0,0,0q�$��0��$
��Bold"�