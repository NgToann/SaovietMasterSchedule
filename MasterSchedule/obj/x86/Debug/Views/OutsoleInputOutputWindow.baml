   M S B A M L   `   `   `  ����  MasterSchedule3    -MasterSchedule.Views.OutsoleInputOutputWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$,  &Master Schedule - Input Outsole Output��5   	   $	��200��68   $	��800��6E    � SizeToContent$ Height=�6Q    � WindowStartupLocation$ CenterScreen=�5   !   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5	      $��* �6   �� 5
      $
��Auto �6   .+�5   
   � $��0��6    � ColumnDefinitions 5      �� 5      ��6#   ��  �� ElementName$ dgOutsoleMaterial�� �� Path$ BorderThickness.Left��� 5      ��6#   �� $ dgOutsoleMaterial��$ RowHeaderWidth��� 5      $��* �6#   .+�5      �� $��3��6   
���   �$��1,1,1,0q�69   ._�5      �� $��0,3,0,3q�6   $
��SIZE��6.    �� TextAlignment$ Center=�6:   $
��Bold"�6Q   5      5      &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dgOutsoleMaterial��5       $��1��6   	   RowHeaderWidth$	 10Q�6;   
   AutoGenerateColumns
 .     CanUserAddRows .     FrozenColumnCount$ 1��5         SelectionUnit$ CellOrRowHeader=�6)    �� AlternationCount$ 2��5         AlternatingRowBackground
 �����   HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����   ColumnHeaderStyle 5      �� 5       �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6   .�5      ��  �� Property � �� �� Value$
 Bold"�63   �� 5        � ��$ Center=�6C   5!         Columns 5#      4  .System.Windows.Controls.DataGridTemplateColumn  5$      -   5        colCompleted��   CellTemplate 5%      �� 5&      .R�5'      ��  ��Name$ btnCompleted��6%   -   5        $��OK��5'   ;   6H   5(      5*      5,      � 5-   
   $��2��6   $��0,5,0,0q�6    5.      �� 5/      $
��Auto �6#   �� 50      $
��Auto �6#   �� 51      $
��Auto �6#   �� 52      $��* �6#   .+�54      �� $��TOTAL: ��6   $��Bottom=�6'   �� 55      -   5         ��Name$ lblQtyTotal��55      $��1��6-   $��Bottom=�6=   $��0��6X   $
��Bold"�6a   $
��Left=�6s   �� 56      -   5        $ lblQtyOrder��56      $��2��6-   $��Bottom=�6=   $��/0��6X   �� 57      -   5        $ btnSave��57      $��3��6&   $	��150��67   $��Right=�6F   $
��Save��6b   $
��Bold"�6q   {�.  6�   58      5:      5;      