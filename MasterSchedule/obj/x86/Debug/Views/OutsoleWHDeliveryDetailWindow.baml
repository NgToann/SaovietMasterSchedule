   M S B A M L   `   `   `  ����  MasterSchedule8    2MasterSchedule.Views.OutsoleWHDeliveryDetailWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$2  ,Master Schedule - Outsole WH Delivery Detail��5   	   $	��480��6>   $	��640��6N    � WindowState$ 	Maximized=�6]    � WindowStartupLocation$ CenterScreen=�6�   .��5   
   �  � RowDefinitions 5      �� 5      $��* �6    �� 5      $
��Auto �6    �� 5	      $
��Auto �6    .+�5      &   System.Windows.Controls.DataGrid  -   5          Name$ dgInventory��5   $   $��0��6      
IsReadOnly .    RowHeaderWidth$ 5Q�6K      SelectionUnit$ CellOrRowHeader=�6^      AutoGenerateColumns .  	   FrozenColumnCount$	 1��5   /   
 �� AlternationCount$
 2��6E      AlternatingRowBackground
 �����   HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����   CanUserReorderColumns .     ColumnHeaderStyle 5      �� 5       �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6   .�5      ��  �� Property � �� �� Value$ Center=�6G   5      5      � 5      -   5         �Name$ 	gridTotal��5      $��1��6&    � ColumnDefinitions 5      �� 5      ��6'   ��  �� ElementName$ dgInventory�� �� Path$ BorderThickness.Left��� 5      ��6'   �� $ dgInventory��$ RowHeaderWidth�5      � 5      $��2��6   $��0,2,0,0q�6     5      �� 5      $
��Auto �6'   �� 5      .+�5       ��  �� Orientation$ 
Horizontal=�6   $��Bottom=�66   $��5,0,0,0q�6Q   $y�	Collapsed=�6b   .+�5!      �� $!��Product Background Legend: ��6    $��0,0,5,0q�6C   �� 5"      $��40��6    
��� � �6+   �� 5#      $��
: Finished��6    $��Italic$�62   $��2,0,20,0q�6E   �� 5$      $��40��6    
��� ���6+   �� 5%      $��: In Process��6    $��Italic$�64   $��2,0,20,0q�6G   �� 5&      $��No color��6    �� 5'      $��: Not yet input��6    $��Italic$�67   $��2,0,20,0q�6J   5(      �� 5)      $��1��6   $ 
Horizontal=�6-   $��Right=�6G   .+�5*      �� -   5         ��Name$ btnShowBalanceOnly��5*      $��0,0,5,0q�69   {�.  6k   �� 5+      -   5        $ btnExcelFile��5+      $��70��63   $��_Excel��6>   {�.  6a   5,      5-      5.      5/      