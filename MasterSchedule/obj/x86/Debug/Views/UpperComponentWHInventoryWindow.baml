   M S B A M L   `   `   `  ����  MasterSchedule:    4MasterSchedule.Views.UpperComponentWHInventoryWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$4  .Master Schedule - Upper Component WH Inventory��5   	   $	��300��6@   $	��400��6P    � WindowStartupLocation$ CenterScreen=�6_   .��5      � $��3q�6    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5
   
   &   System.Windows.Controls.DataGrid  -   5          Name$ dgInventory��5
         RowHeaderWidth$ 5Q�6(      
IsReadOnly .    AutoGenerateColumns .   �� AlternationCount$ 2��5         AlternatingRowBackground
 �����	   HorizontalGridLinesBrush
	 �����
   VerticalGridLinesBrush

 �����   CanUserReorderColumns .     Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      -   5        Column1��,  &System.Windows.Controls.DataGridColumn   Header$ O/S Code��5   6      
FontWeight$
 Bold"�6h      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6H   �� *OutsoleCode+6z     5      -   5        Column2��$ 	Total Qty��5   6    .  6I   �� *
Quantity+6f     5      -   5        Column3��$ Matching��5   6    .  6H   �� *
Matching+6e   5      � 5   
   $��1��6    � ColumnDefinitions 5      �� 5      ��6#   ��  �� ElementName$ dgInventory�� �� Path$ BorderThickness.Left��� 5      ��6#   �� $ dgInventory��$ RowHeaderWidth��� 5      ��6#   �� $ Column1��$ ActualWidth��� 5      ��6#   �� $ Column2��$ ActualWidth��� 5      ��6#   �� $ Column3��$ ActualWidth�.+�5      �� $��2��6   $��1,0,1,1q�6%   
���   �._�5       �� $��1,0,0,0q�6   $��TOTAL��6-   $
��Bold"�6:   5!      �� 5"      $��3��6   $��0,0,1,1q�6%   
���   �._�5#      �� -   5         ��Name$ lblTotalQTy��5#      $��1,0,0,0q�61   $��0��6B   $
��Bold"�6K   5$      �� 5%      $��4��6   $��0,0,1,1q�6%   
���   �._�5&      �� -   5        $ lblMatching��5&      $��1,0,0,0q�61   $��0��6B   $
��Bold"�6K   5'      5+      5,      5-      