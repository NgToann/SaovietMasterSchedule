   M S B A M L   `   `   `  ����  MasterSchedule?    9MasterSchedule.Views.AddOutsoleMaterialRejectToRackWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Add Reject To Rack��5   	    � SizeToContent$ WidthAndHeight=�6$    � WindowStartupLocation$ CenterScreen=�6C    � WindowStyle$ 
ToolWindow=�6h    � 
ResizeMode$ NoResize=�6�   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5	      $
��Auto �6   �� 5
      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� -   5         ��Name$ txtSupplier��5      $��18(�6)    �� TextAlignment$ Center=�67   $
��Bold"�6N   $��Italic$�6`   �� 5   
   $��1��6    �� Orientation$ 
Horizontal=�6"   $��0,5,0,0q�6;   .+�5      �� $��Select Rack: ��6   $��Bottom=�6-   $��80��6H   $��13(�6S   �� 5      -   5        	 ��Name$	 cbRack��5      $	��150��6'   $��14(�63   6A   5      �� 5   
   $��2��6   $ 
Horizontal=�6"   $��0,5,0,0q�6;   .+�5      �� $��
Carton No:��6   $��Bottom=�6*   $��80��6E   $��13(�6P   �� 5      -   5        
 ��Name$
 txtCartonNumber��5      $	��150��6/   $��14(�6;   {�.  6I   5      &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dgSizeNoAndQuantity��5       $��3��6      RowHeaderWidth$ 0Q�6=   $��0,5,0,0q�6P      AutoGenerateColumns .     CanUserAddRows .     FrozenColumnCount$ 1��5   F      SelectionUnit$ CellOrRowHeader=�5       �� AlternationCount$ 2��63      AlternatingRowBackground
 �����   HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����   ColumnHeaderStyle 5      �� 5       �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6   .�5      ��  �� Property � �� �� Value$
 Bold"�63   5      5       � 5!   
   $��5��6   $��0,5,0,0q�6    � ColumnDefinitions 5"      �� 5#      $��2* �6#   �� 5$      $��* �6#   .+�5&      �� -   5         ��Name$ btnSave��5&      $��_Save��6&   $
��Bold"�66   6H   5'      5(      5)      