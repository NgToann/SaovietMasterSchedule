   M S B A M L   `   `   `  ����  MasterSchedule?    9MasterSchedule.Views.AddOutsoleMaterialRackPositionWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � 
ResizeMode$  CanMinimize=�5   @   $	��300��6Y    � Title$ Add Rack��5   	    � SizeToContent$ WidthAndHeight=�6    � WindowStartupLocation$ CenterScreen=�69    � WindowStyle$ 
ToolWindow=�6^   .��5      � $��2q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      .+�5
   
   ��    GroupBoxTemplate#���   6   ��5      �� 5        GroupBackground#���  6     BorderStyle#���  6T    �� Padding$ 	10,5,10,5q�6�   ._�5      �� -   5         ��Name$ txtSupplier��5       $��18(�65    �� TextAlignment$ Center=�6C   $��SemiBold"�6Z   $��Italic$�6p   
���  ��6�   5      .��5      �  5      �� 5      $
��auto �6$   �� 5      $
��auto �6$   �� 5      $
��auto �6$   �� 5      $
��auto �6$   �� 5      $
��auto �6$   �� 5      $
��auto �6$   .+�5      �� $��1��6   	 �� Orientation$	 
Horizontal=�6*   $��0,5,0,0q�6C   .+�5      �� $��Rack No:��6    $��Bottom=�60   $��70��6K   $��13(�6V   �� 5      -   5        
 ��Name$
 txtRackNumber��5      $	��150��65   $��18(�6D   $��Center=�6R   $��3q�6t   5      �� 5      $��2��6   $	 
Horizontal=�6*   $��0,5,0,0q�6C   .+�5      �� $��
Carton No:��6    $��Bottom=�62   $��70��6M   $��13(�6X   �� 5      -   5        $
 txtCartonNumber��5      $	��150��67   $��18(�6C   $��Center=�6Q   $��3q�6s   5       &   System.Windows.Controls.DataGrid  5!      -   5          Name$ dgSizeNoAndQuantity��5!   (   $��3��6      RowHeaderWidth$ 0Q�6E   $��0,5,0,0q�6X      AutoGenerateColumns .     CanUserAddRows .     FrozenColumnCount$ 1��5"   J      SelectionUnit$ CellOrRowHeader=�5#       �� AlternationCount$ 2��67      AlternatingRowBackground
 �����$��13(�5$   <      HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����   ColumnHeaderStyle 5&      �� 5'       �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6    .�5(      ��  �� Property � �� �� Value$ Center=�6K   �� 5)       � ��$ 14(�69   �� 5*       � ��
 �����6;   �� 5+       � ��
 �   �6;   �� 5,       � ��
 �����6<   �� 5-       � ��$ 0.5,0.5,0.5,0q�6@   �� 5.       � ��$ 4q�68   5/      51      � 52      $��5��6   $��0,15,0,2q�6%    � ColumnDefinitions 53      �� 54      $��2* �6+   �� 55      $��* �6+   .+�57      �� -   5         ��Name$ btnAdd��57      $
��_Add��6-     ButtonTemplate#���  6<    � Cursor$
 Hand��6�   �� 58      -   5        $ 	btnRemove��58      $��2,0,0,0q�60   $��1��6A   #���  6R   $��_Remove��6�   6�   59      5:      5;      5<      5=      