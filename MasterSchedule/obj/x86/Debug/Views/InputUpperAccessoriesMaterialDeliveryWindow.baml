   M S B A M L   `   `   `  ����I  EMasterSchedule, Version=1.3.0.4, Culture=neutral, PublicKeyToken=null;"clr-namespace:MasterSchedule.ViewsMasterSchedule.Views  5   	   F    @MasterSchedule.Views.InputUpperAccessoriesMaterialDeliveryWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:MasterSchedule.Views     5   	   -   5          � WindowStartupLocation$  CenterScreen=�5   1    � Title$A ;Master Schedule - Input Upper Accessories Material Delivery��5   	   $	��600��6M   $	��900��6Z   .��5	      � $��2q�6    � RowDefinitions 5
   
   �� 5      $
��auto �6   �� 5      �� 5      $
��auto �6   .+�5   
   �� -   5         ��Name$ tblTitle��5      $%��Upper Accessories Delivery PO: ��6&   $��18(�6M   $��Center=�6[   $��Center=�6x   $��SemiBold"�6�   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dgDeliveryInfo��5       $��1��6      AutoGenerateColumns .     
IsReadOnly .     SelectionUnit$ CellOrRowHeader=�6g      CanUserAddRows .  	   CanUserSortColumns	 .  
   RowHeaderWidth$	
 NaNQ�5   U   $��13(�6j      HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����   FrozenColumnCount$ 3��5   [      ColumnHeaderStyle 5      �� 5       �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6   .�5      ��  �� Property � �� �� Value$ Center=�6C   �� 5       � ��$ 14(�61   �� 5       � ��$ SemiBold"�63   �� 5       � ��
 �����63   �� 5       � ��
 �   �63   �� 5       � ��
 �����64   �� 5       � ��$ 0.5,0.5,0.5,0.5q�68   �� 5       � ��$ 4,3,4,3q�60   5       � 	Resources 5      *  $System.Windows.Controls.DataGridCell �� Triggers P� Property P� Value��%�   '        �� 5       "  6    5!      P� 5"       �@ 
IsSelected . . �5#      ��  � ��
 ��� �6;   �� 5$       � ��
 �   �6;   5%      5'      5(      5)      � 5*   
   $��2��6   $��0,2,0,0q�6    � ColumnDefinitions 5+      �� 5,      �� 5-      $
��auto �6#   �� 5.      $
��auto �6#   .+�50      �� $
��Left=�6    �� Orientation$ 
Horizontal=�64   $��Center=�6M   .+�51      �� $��20��6   $��20��6#   $��/Images/viewDetail.png��6/   52      �� 53      -   5         ��Name$ 	btnDelete��53      $	��100��6(    � Cursor$
 Hand��67      ButtonTemplate#���   6E   $��1��6}   ��54      �� 55      $��
  Delete  ��6    
���  ��$��14(�6C   57      �� 58      -   5        $ btnSave��58      $��80��6&   $��10,0,0,0q�64   $
 Hand��6F   #���   6T   $��  Save  ��6�   $��2��6�   6�   59      5:      5;      