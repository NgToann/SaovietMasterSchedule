   M S B A M L   `   `   `  ����  MasterSchedule5    /MasterSchedule.Views.OutsoleInputMaterialWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � WindowState$  	Maximized=�5   U    � Title$. (Master Schedule - Input Outsole Material��5   	   $	��400��6:   $
��1000��6J    � WindowStartupLocation$ CenterScreen=�5   	   .��5      �� $
��Auto=�6   $
��Auto=�68   $��2q�6[   .��5   
   � $��2q�6    � RowDefinitions 5	      �� 5
      $
��Auto �6    �� 5      $��* �6    �� 5      $
��Auto �6    .+�5      � $��0��6    � ColumnDefinitions 5      �� 5      ��6'   ��  �� ElementName$ dgOutsoleMaterial�� �� Path$ BorderThickness.Left��� 5      ��6'   �� $ dgOutsoleMaterial��$ RowHeaderWidth��� 5      ��6'   �� $ colSuppliers��$ ActualWidth��� 5      ��6'   �� $ colETD��$ ActualWidth��� 5      ��6'   �� $ colActualDate��$ ActualWidth��� 5      $��* �6'   .+�5      � $��5��6    5      �� 5      $
��auto �6+   �� 5      $��10 �6+   �� 5      $��* �6+   .+�5      �� $��O/S Material��6    $��Center=�64   
��� E��$��Italic$�6f   $��SemiBold"�6y   $��14(�6�   �� 5      -   5         ��Name$ stkIncludeReject��5   H   $y�	Collapsed=�6!   $��2��68    �� Orientation$ 
Horizontal=�6b   $��Right=�6{   .+�5      	� -   5        	 	�Name$	 radInputReject��5   &   $��Include Reject��6>      RadioBlueStyle#���   6W   
 	� 	GroupName$
 InputReject��6�   $��Center=�6�   6�   	� 5       -   5        $	 radNotInputReject��5    9   $��	10,0,10,0q�6&   $	��Not��6T     RadioRedStyle#���  6b   $
 InputReject��6�   $��Center=�6�   6�   5!      5"      �� 5#      $��5��6   
���   �$��1,1,1,0q�6=   ._�5$      �� $��0,3,0,3q�6    $
��SIZE��62    �� TextAlignment$ Center=�6>   $
��Bold"�6U   5%      5&      � 5'      $��1��6    5(      �� 5)      $
��auto �6$   �� 5*      $
��auto �6$   �� 5+      $
��auto �6$   .+�5-      &   System.Windows.Controls.DataGrid  -   5          Name$ dgOutsoleMaterial��5-         RowHeaderWidth$ 10Q�66   $��13(�6J      AutoGenerateColumns .     CanUserAddRows .     FrozenColumnCount$ 1��5.   F      SelectionUnit$ CellOrRowHeader=�5/       �� AlternationCount$ 2��63      AlternatingRowBackground
 �����   CanUserSortColumns .     HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����   ColumnHeaderStyle 52      �� 53       �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6    .�54      ��  �� Property � �� �� Value$
 Bold"�6;   �� 55       � ��$ Center=�6K   56         Columns 58      4  .System.Windows.Controls.DataGridComboBoxColumn  59      ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$	 120 61   -   5        colSuppliers��   Header$ 	Suppliers��59   S      DisplayMemberPath$
 Name��6f      SelectedValuePath$ OutsoleSupplierId��6       
IsReadOnly  . !   SelectedItemBinding! 6�   �� *
Supplier+6�   0  *System.Windows.Controls.DataGridTextColumn  5:      $ 60 6-   -   5        
colETD��$	 EFD��5:   `     . 1  +System.Windows.Controls.DataGridBoundColumn"   Binding" 6H   �� *ETD+6m     5;      $ 80 6-   -   5        colActualDate��$- 'OrderSize
O/S Size
M/S Size
Actual Date��5;   n     . " 6O   �� *
ActualDate+6�   4  .System.Windows.Controls.DataGridTemplateColumn  5<      -	   5        colCompleted��#   CellTemplate# 5=      �� 5>   "   .R�5?   &   �� $ ��Name$$ btnCompleted��6-   -
   5        $��OK��5?   C   6P   5@   #   5B        5C      -   5        
colAddRack��# 5D      �� 5E   "   .R�5F   &   �� $$ 
btnAddRack��6-   -   5        $��...Rack��5F   A   $
��Bold"�6S   $��Italic$�6e   6x   5G   #   5I      5K      � 5M      $��1��6   $��0,5,0,0q�6$    5N      �� 5O      ��6+   �� $ dgOutsoleMaterial��$ BorderThickness.Left��� 5P      ��6+   �� $ dgOutsoleMaterial��$ RowHeaderWidth��� 5Q      ��6+   �� $ colSuppliers��$ ActualWidth��� 5R      ��6+   �� $ colETD��$ ActualWidth��� 5S      ��6+   �� $ colActualDate��$ ActualWidth��� 5T      $
��auto �6+   .+�5V      �� $��Release Painting��6    $��1��68   $��2��6H   $��Bottom=�6\   
���  ��$��Italic$�6�   $��SemiBold"�6�   $��14(�6�   �� 5W      -   5        % ��Name$% cboPaintingSupplier��5W      $��3��6<   $��2��6L   & �� DisplayMemberPath$
& Name��6`   ' �� SelectedValue$' OutsoleSupllierId��6y   $��Center=�6�   6�   5X      � 5Z      $��2��6   $��0,2,0,0q�6$    5[      �� 5\      ��6+   �� $ dgOutsoleMaterial��$ BorderThickness.Left��� 5]      ��6+   �� $ dgOutsoleMaterial��$ RowHeaderWidth��� 5^      ��6+   �� $ colSuppliers��$ ActualWidth��� 5_      ��6+   �� $ colETD��$ ActualWidth��� 5`      ��6+   �� $ colActualDate��$ ActualWidth��� 5a      $
��auto �6+   .+�5c        -   5        $ dgReleasePainting��5c      $ 10Q�6:   $��6��6N    .  $��13(�6~    .  $ 1��5d   6   $ CellOrRowHeader=�6L   $ 2��6l   
 ����� .  
 �����
 ����� 5g      �� 5h      "  6$   .�5i   "   ��  � ��$
 Bold"�6?   �� 5j   "    � ��$ Center=�6O   5k      5n      5o      5p      � 5q      $��2��6   $��0,2,0,0q�6     5r      �� 5s      $
��Auto �6'   �� 5t      $��* �6'   �� 5u      $
��Auto �6'   .+�5w      �� -   5        $$ btnSave��5w      $��2��6*   $	��150��6�   $��Right=�6�   $
��Save��6�   $
��Bold"�6�   {�.  ��6:   �� $ cboPaintingSupplier��$ ActualHeight�6�   5x      5y      5z      5{      