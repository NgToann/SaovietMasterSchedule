   M S B A M L   `   `   `  ����I  EMasterSchedule, Version=1.2.8.9, Culture=neutral, PublicKeyToken=null;"clr-namespace:MasterSchedule.ViewsMasterSchedule.Views  5   	   ,    &MasterSchedule.Views.EditMachineWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:MasterSchedule.Views     5   	   -   5        $��14(�5        � WindowStartupLocation$  CenterScreen=�6(    � Title$$ Master Schedule - Edit Machine��5   	   $	��300��60   $	��500��6=   .��5	      � -   5         �Name$ gridMain��5	      $��5q�6    � ColumnDefinitions 5
   
   �� 5      $��2* �6   �� 5      $��5 �6   �� 5      $��3* �6    � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $��* �6   .+�5   
   �� $��Phase��6   $��Center=�6!   �� 5   
   -   5         ��Name$ cbPhase��5      $��2��6$    �� DisplayMemberPath$ 	PhaseName��5       �� SelectedValue$ PhaseID��5      $��Center=�5   :    �� SelectedItem 6   �� *PhaseSelected+6:   �� 5   
   $��
Machine ID��6   $��1��6&   $��Bottom=�63   �� 5   
   $��1��6   $��2��6   $��0,5,0,0q�6J   	 �� 
IsReadOnly	 . 
���   �{�.  $��Center=�6�   ��6/   �� *	MachineID+6�   �� 5   
   $��Machine Name��6   $��2��6(   $��Bottom=�65   �� 5    
   $��2��6   $��2��6   $��0,5,0,0q�6L   
���   �$��Center=�6q   ��6/   �� *MachineName+6q   �� 5"   
   $��Available Quantity��6   $��3��6.   $��Bottom=�6;   �� 5#   
   -   5        $��3��5#      $��2��6   $��0,5,0,0q�6J   
���   �$��Center=�6o   ��6/   �� *	Available+6�   �� 5%   
   $��0,5,0,0q�6   $��4��6$   $��Center=�61   
 d� 	IsChecked
 6S   �� *	IsMachine+��5&      �� 5'      $��Is Machine ?��6   $��Center=�60   $��Italic$�6K   $��SemiBold"�6^   5)      � 5*   
   $��5��6    5+      �� 5,      $
��auto �6    �� 5-      .+�5/      �� $��Remarks��6   $	��Top=�6'   $��0,5,0,0q�6?   � 50      $��1��6    51      �� 52      �� 53      $��5 �6'   �� 54      .+�56      �� -   5         ��Name$ 	btnUpdate��56      $	��Add��6,   $��25��6:   $��Bottom=�6F   6a   �� 57      -   5        $ 	btnDelete��57      $��Delete ��6,   $��25��6>   $��2��6J   $��Bottom=�6Z   6u   58      59      �� 5:   
   $��6��6   $��2��6    �� TextWrapping$
 Wrap=�6H   
���   ���6/   �� *	Remarks+6\   5;      5<      