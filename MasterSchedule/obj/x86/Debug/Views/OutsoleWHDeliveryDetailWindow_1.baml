   M S B A M L   `   `   `  ����  MasterSchedule:    4MasterSchedule.Views.OutsoleWHDeliveryDetailWindow_1   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � WindowStartupLocation$  CenterScreen=�5   @    � WindowState$ 	Maximized=�6e   $��14(�5   	    � Title$2 ,Master Schedule - Outsole WH Delivery Detail��5   	   $	��500��6>   $	��800��6K   .��5      � $��2q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5	      �� 5
      $
��Auto �6   .+�5   
   �� -   5         ��Name$ txtTitle��5      $��Center=�6&   $��Center=�6A   $��SemiBold"�6^   $��Times New Roman)�6t   $��20(�6�   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dgWHDeliveryDetail��5   1   $��1��6   $��0,5,0,0q�6       AutoGenerateColumns .   �� AlternationCount$ 2��6i      AlternatingRowBackground
 �����	   CanUserSortColumns	 . 
   CanUserAddRows
 .     SelectionUnit$ FullRow=�5   .      HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����   ClipboardCopyMode$ IncludeHeader=�6�      
IsReadOnly . $��Times New Roman)�5   )   $��14(�6F      FrozenColumnCount$ 8��6T      RowHeaderWidth$	 NaNQ�6�      	CellStyle 5      �� 5       �� 
TargetType*  $System.Windows.Controls.DataGridCell"  6    �� Triggers 5      P� 5       P� Property �@ 
IsSelected P� Value . . �5      ��  �� Property � �� �� Value
 �   �6;   �� 5       � ��
 ��� �6;   �� 5       � ��
 ���� 6;   �� 5       � ��$ Semibold"�6;   �� 5       � ��$ 0,0,0,0q�6@   5      5         ColumnHeaderStyle 5      �� 5      =  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6   .�5      ��  � ��$ 15(�61   �� 5        � ��
 �����63   �� 5!       � ��
 �����64   �� 5"       � ��$ 0.5,0.5,0.5,0q�68   �� 5#       � ��$ 8,5,5,8q�60   �� 5$       � ��$ Center=�6C   5%      5'      � 5(   
   $��2��6   $��0,2,0,0q�6    � ColumnDefinitions 5)      �� 5*      $��* �6#   �� 5+      $
��auto �6#   .+�5-      �� .@�5.      �� $��Processing:��6   � 5/      -   5         �Name$ progressBar��5/      $	��100��63   $��15��6?    � Minimum$ 0Q�6K    � Value$ 0Q�6W   �� 50      -   5        $ 	lblStatus��50      $�� ��6/   $
��Bold"�67   51      �� 52      -   5         ��Name$ btnExcelExport��52      $��1��6-   $��  Export Excel  ��6=   {�.  6u   53      54      55      