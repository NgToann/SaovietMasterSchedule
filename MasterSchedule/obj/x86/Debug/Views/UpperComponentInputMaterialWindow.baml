   M S B A M L   `   `   `  ����  MasterSchedule<    6MasterSchedule.Views.UpperComponentInputMaterialWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$-  'Master Schedule - Input Upper Component��5   	   $
��Auto��69   $	��800��6G    � SizeToContent$ Height=�6S    � WindowStartupLocation$ CenterScreen=�6�   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5	      $
��Auto �6   .+�5   
   � $��0��6    � ColumnDefinitions 5      �� 5      ��6#   ��  �� ElementName$ dgUpperComponent�� �� Path$ BorderThickness.Left��� 5      ��6#   �� $ dgUpperComponent��$ RowHeaderWidth��� 5      ��6#   �� $ colUpperComponents��$ ActualWidth��� 5      ��6#   �� $ colETD��$ ActualWidth��� 5      ��6#   �� $ colActualDate��$ ActualWidth��� 5      $��* �6#   .+�5      �� $��5��6   
���   �$��1,1,1,0q�69   ._�5      �� $��0,3,0,3q�6   $
��SIZE��6.    �� TextAlignment$ Center=�6:   $
��Bold"�6Q   5      5      &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dgUpperComponent��5       $��1��6   	   RowHeaderWidth$	 10Q�6:   
   AutoGenerateColumns
 .     CanUserAddRows .     FrozenColumnCount$ 1��5   F      SelectionUnit$ CellOrRowHeader=�5       �� AlternationCount$ 2��63      AlternatingRowBackground
 �����   HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����   ColumnHeaderStyle 5      �� 5       �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6   .�5      ��  �� Property � �� �� Value$
 Bold"�63   �� 5        � ��$ Center=�6C   5!         Columns 5#      4  .System.Windows.Controls.DataGridComboBoxColumn  5$      -   5        colUpperComponents��,  &System.Windows.Controls.DataGridColumn   Header$ Upper Component��5$   E      DisplayMemberPath$ UpperComponentName��6^      SelectedValuePath$ UpperComponentID��6�      
IsReadOnly .    DisplayIndex$ 1��6�      SelectedItemBinding 6�   �� *UpperComponent+6�   0  *System.Windows.Controls.DataGridTextColumn  5%      -   5        
colETD��$	 EFD��5%   M    . $ 2��6l   1  +System.Windows.Controls.DataGridBoundColumn   Binding 65   �� *ETD+6l     5&      -   5        colActualDate��$ Actual Date��5&   [    . $ 3��6�    6<   �� *
ActualDate+6�   4  .System.Windows.Controls.DataGridTemplateColumn  5'      -   5        colCompleted��   CellTemplate 5(      �� 5)      .R�5*      ��  ��Name$ btnCompleted��6%   -   5        $��OK��5*   ;   6H   5+      5-      5/      �� 50   
   -   5        $ btnSave��50      $��2��6"   $��0,2,0,0q�6/   $	��150��6@   $��Right=�6O   $
��Save��6k   $
��Bold"�6z   {�.  6�   51      52      