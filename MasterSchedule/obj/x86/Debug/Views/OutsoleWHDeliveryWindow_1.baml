   M S B A M L   `   `   `  ����  MasterSchedule4    .MasterSchedule.Views.OutsoleWHDeliveryWindow_1   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � WindowStartupLocation$  CenterScreen=�5   @    � WindowState$ 	Maximized=�6e    � Title$+ %Master Schedule - Outsole WH Delivery��5   	   $	��400��67   $	��750��6D   .��5      � $��2q�6    � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      �� 5	      $
��auto �6   .+�5   
   �� $(��"OUTSOLE WAREHOUSE DELIVERY SUMMARY��6   $��Center=�6>   $��Center=�6Y   $��20(�6v   $��Times New Roman)�6�   $��SemiBold"�6�   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dgWHDelivery��5   1   $��1��6   $��0,5,0,0q�6       AutoGenerateColumns .   �� AlternationCount$ 2��6c      AlternatingRowBackground
 �����   CanUserSortColumns . 	   CanUserAddRows	 .  
   SelectionUnit$

 Cell=�5   .      HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����   
IsReadOnly . $��Times New Roman)�5   )   $��14(�6F      FrozenColumnCount$ 6��6T      	CellStyle 5      �� 5       �� 
TargetType*  $System.Windows.Controls.DataGridCell"  6    �� Triggers 5      P� 5       P� Property �@ 
IsSelected P� Value . . �5      ��  �� Property � �� �� Value
 �   �6;   �� 5       � ��
 ��� �6;   �� 5       � ��$ Semibold"�6;   �� 5       � ��$ 0,0,0,0q�6@   5      5         ColumnHeaderStyle 5      �� 5      =  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6   .�5      ��  � ��$ Center=�6C   �� 5       � ��$ 15(�61   �� 5       � ��
 �����63   �� 5        � ��
 �����64   �� 5!       � ��$ 0.5,0.5,0.5,0q�68   �� 5"       � ��$ 8,5,5,8q�60   5#      5Q      �� 5R   
   $��2��6   .@�5S      �� $��Processing:��6   � 5T      -   5         �Name$ progressBar��5T      $	��100��6/   $��15��6;    � Minimum$ 0Q�6G    � Value$ 0Q�6S   �� 5U      -   5         ��Name$ 	lblStatus��5U      $�� ��6+   $
��Bold"�63   5V      5W      5X      