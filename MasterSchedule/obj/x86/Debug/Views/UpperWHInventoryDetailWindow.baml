   M S B A M L   `   `   `  ����  MasterSchedule7    1MasterSchedule.Views.UpperWHInventoryDetailWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$1  +Master Schedule - Upper WH Inventory Detail��5   	   $	��480��6=   $	��640��6J    � WindowStartupLocation$ CenterScreen=�6m    � WindowState$ 	Maximized=�6�   .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5
   
   &   System.Windows.Controls.DataGrid  -   5          Name$ dgInventory��5
         
IsReadOnly .    RowHeaderWidth$ 5Q�6:      AutoGenerateColumns .     FrozenColumnCount$ 1��5   /   	 �� AlternationCount$	 2��6E   
   AlternatingRowBackground

 �����   HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����   CanUserReorderColumns .     Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      -   5        Column1��,  &System.Windows.Controls.DataGridColumn   Header$ PO No.��5   6      
FontWeight$
 Bold"�6d      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6F   �� *	ProductNo+6v     5      -   5        Column2��$ Style��5   6    .  6E   �� *
ShoeName+6b     5      -   5        Column3��$ Article No.��5   6    .  6K   �� *	ArticleNo+6i     5      -   5        Column4��$	 EFD��5   6   $
 Bold"�6p    .  6C   �� *ETD+ �� StringFormat$ dd-MMM��6�     5      -   5        Column5��$ Quantity��5   6    .  6H   �� *
Quantity+6e     5      -   5        Column6��$ Assy Release��5   6    .  6L   �� *ReleaseQuantity+6p     5      -	   5        Column7��$ Upper��5   6    .  6E   �� *SewingOutput+6f     5      -
   5        Column8��$ Outsole��5   6    .  6G   �� *OutsoleOutput+6i     5      -   5        Column9��$ Matching��5   6    .  6H   �� *
Matching+6e     5      -   5        Column10��$ O/S Mats Arrival��5   7    .  6Q   �� *OSMaterialArrival+   	CellStyle 5      �� 5       �� 
TargetType*  $System.Windows.Controls.DataGridCell"  6    .�5      ��  �� Property � �� �� Value 6;   �� *OSMaterialArrivalForeground+5      5      5       � 5!   
   $��1��6    � ColumnDefinitions 5"      �� 5#      ��6#   ��  �� ElementName$ dgInventory�� �� Path$ BorderThickness.Left��� 5$      ��6#   �� $ dgInventory��$ RowHeaderWidth��� 5%      ��6#   �� $ Column1��$ ActualWidth��� 5&      ��6#   �� $ Column2��$ ActualWidth��� 5'      ��6#   �� $ Column3��$ ActualWidth��� 5(      ��6#   �� $ Column4��$ ActualWidth��� 5)      ��6#   �� $ Column5��$ ActualWidth��� 5*      ��6#   �� $ Column6��$ ActualWidth��� 5+      ��6#   �� $ Column7��$ ActualWidth��� 5,      ��6#   �� $ Column8��$ ActualWidth��� 5-      ��6#   �� $ Column9��$ ActualWidth�.+�5/      �� $��2��6   $��4��6%   $��1,0,1,1q�69   
���   �._�50      �� $��1,0,0,0q�6   $��TOTAL��6-   $
��Bold"�6:   51      �� 52      $��6��6   $��0,0,1,1q�6%   
���   �._�53      �� -   5         ��Name$ lblQuantityTotal��53      $��1,0,0,0q�66   $��0��6G   $
��Bold"�6P   54      �� 55      $��7��6   $��0,0,1,1q�6%   
���   �._�56      �� -   5        $ lblReleaseQuantityTotal��56      $��1,0,0,0q�6=   $��0��6N   $
��Bold"�6W   57      �� 58      $��8��6   $��0,0,1,1q�6%   
���   �._�59      �� -   5        $ lblSewingOutput��59      $��1,0,0,0q�65   $��0��6F   $
��Bold"�6O   5:      �� 5;      $��9��6   $��0,0,1,1q�6%   
���   �._�5<      �� -   5        $ lblOutsoleOutput��5<      $��1,0,0,0q�66   $��0��6G   $
��Bold"�6P   5=      �� 5>      $��10��6   $��0,0,1,1q�6&   
���   �._�5?      �� -   5        $ lblMatching��5?      $��1,0,0,0q�61   $��0��6B   $
��Bold"�6K   5@      5A      5B      5C      