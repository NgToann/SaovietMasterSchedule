   M S B A M L   `   `   `  ����I  EMasterSchedule, Version=1.2.8.9, Culture=neutral, PublicKeyToken=null;"clr-namespace:MasterSchedule.ViewsMasterSchedule.Views  5   	   5    /MasterSchedule.Views.AddRejectForMaterialWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:MasterSchedule.Views     5   	   -   5        $��13(�5        � WindowStartupLocation$  CenterScreen=�6(    � 
ResizeMode$ CanMinimize=�6M    � SizeToContent$ WidthAndHeight=�6f    � Title$5 /Master Schedule - Add/Update Accessories Reject��5   	   $	��600��6B   $	��600��6Q   .��5	      � $��2q�6    � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��1��6   $��0,2,0,0q�6       GroupBoxTemplate#���   61   ��5      �� 5        GroupBackground#���  6     BorderStyle#���  6T   ._�5      �� -   5         ��Name$ tblGroupHeader��5       $��List of Defects��68   
���  ��$��14(�6`   $��Italic$�6n    �� Padding$ 	10,0,10,0q�6�   5      .��5      �  5      �� 5      $��* �6$   �� 5      $
��auto �6$   .+�5      � -   5         �Name$ 	gridError��5       � ColumnDefinitions 5      �� 5      �� 5      �� 5      �� 5       5"      &   System.Windows.Controls.DataGrid  5#      -   5        	  Name$	 dgReject��5#      $��1��6-   $��0,2,0,0q�6:   
   AutoGenerateColumns
 .   �� AlternationCount$ 2��6g      AlternatingRowBackground
 �����   CanUserSortColumns .     CanUserAddRows .     SelectionUnit$
 Cell=�5$   .      HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����   RowHeaderWidth$ 0Q�6�   $��Times New Roman)�5%      $��17(�64      ColumnHeaderStyle 5&      �� 5'       �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6    .�5(      ��  �� Property � �� �� Value$ Center=�6K   �� 5)       � ��$ SemiBold"�6;   �� 5*       � ��$ 15(�69   �� 5+       � ��
 �����6;   �� 5,       � ��
 �  ��6;   �� 5-       � ��
 �����6<   �� 5.       � ��$ 0.5,0.5,0.5,0q�6@   �� 5/       � ��$ 5q�68   50      52      53      54      � 55   
   $��2��6   $��0,2,0,0q�6   $	��200��6-    56      �� 57      �� 58      $��10 �6#   �� 59      .+�5;      ��  �� Orientation$ Vertical=�6   .+�5<      �� $��Center=�6   $��#SizeNo��69   $��20(�6H   �� 5=      -   5         ��Name$ 	txtSizeNo��5=      $��50(�6-   $��Center=�6;   $��Center=�6_   
���   �$��1,1,1,1q�6�   5>      � 5N      $��2��6    5O      �� 5P      �� 5Q      $
��Auto �6$    5S      �� 5T      $��* �6'   �� 5U      $
��Auto �6'   �� 5V      $
��Auto �6'   .+�5X      �� -   5         ��Name$ 	btnDelete��5X      $��1��6,   $��1��69   $��    Delete    ��6I     ButtonTemplate#���  6b    � Cursor$
 Hand��6�   6�   �� 5Y      -   5        $ btnSave��5Y      $��1��6*   $��2��67   $��10,0,0,0q�6G   $��      Save      ��6Y   #���  6t   $
 Hand��6�   6�   5Z      5[      5]      5^      