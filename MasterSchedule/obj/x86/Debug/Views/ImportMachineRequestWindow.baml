   M S B A M L   `   `   `  ����  MasterSchedule5    /MasterSchedule.Views.ImportMachineRequestWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5        $��14(�5   @     � Title$'  !Master Schedule - Machine Request��5   	   $	��330��63   $	��750��6C    � SizeToContent$ WidthAndHeight=�6R    � WindowStartupLocation$ CenterScreen=�6q   .��5      � $��2q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5	      $
��Auto �6   �� 5
      $��* �6   .+�5   
   �� -   5         ��Name$ 	txtStatus��5      $��CREATE MACHINE REQUEST��6'   $
��Bold"�6E   $��Center=�6W   $��Center=�6r   $��18(�6�   �� 5   
   $��1��6    �� Orientation$ 
Horizontal=�6#   .+�5      �� $��Phase: ��6   $��Bottom=�6'   $��0,0,5,0q�6B   $��SemiBold"�6S   �� 5      -   5         ��Name$ cbPhase��5   #   $	��150��6    �� DisplayMemberPath$ 	PhaseName��64    �� SelectedValue$ PhaseID��6R   $��Center=�6�   $��Center=�6�   	 �� SelectedValuePath	 6j   �� 
 �� Path$
 
PhaseModel�6�   5      �� 5   
   $��2��6   $��0,5,0,0q�6       GroupBoxTemplate#���   61   ��5      �� 5       �� Padding$ 4q�6     Background1#���  6%   ._�5      �� $��Input Order information��6    5      .��5      � $��0,4,0,4q�6    � ColumnDefinitions 5      �� 5      �� 5      $��2* �6'   �� 5      .+�5      �� $ Vertical=�6   $��4,0,4,0q�64   .+�5      �� $��ArticleNo (exclude color)��6    $��Bottom=�6A   $��SemiBold"�6\   �� 5      -   5         ��Name$ txtArticleNo��5      $��Center=�64   
���   �6j   5       �� 5!      $ Vertical=�6   $��1��64   $��4,0,4,0q�6D   .+�5"      �� $��	Shoe Name��6    $��Bottom=�61   $��SemiBold"�6L   �� 5#      $��Center=�68   
���   ���6   �� *
ShoeName+6Z   5$      �� 5%      $��2��6   $ Vertical=�6-   $��4,0,4,0q�6D   .+�5&      �� $��	PatternNo��6    $��Bottom=�61   $��SemiBold"�6L   �� 5'      $��Center=�69   
���   ���6   �� *	PatternNo+6[   5(      5*      5+      �� 5,   
   -   5         ��Name$ 
grbMachine��5,      $��3��6'   #���   64   ��5-      �� 5.      $ 4q�6   #���  6%   ._�5/      �� $!��Input Machine Request for: ��6    50      .��52      �  53      �� 54      $
��auto �6$   �� 55      $��* �6$   .+�57      � -   5         �Name$ gridMachine��57   (   $��0,4,0,4q�6    58      �� 59      �� 5:      �� 5;      �� 5<      5>      � 5?      $��1��6   $��4,10,4,0q�6$   $��28��66   $��Bottom=�6B    5@      �� 5A      $��* �6+   �� 5B      $
��Auto �6+   �� 5C      $
��Auto �6+   .+�5E      �� $ 
Horizontal=�6!   .+�5F      �� $��Worker��6$   $��Bottom=�62   $��SemiBold"�6M   �� 5G      $	��150��6"   
���   �$��10,0,0,0q�6Z   $��Center=�6l   ��6.   �� *Worker+6l   5H      �� 5I      -   5         ��Name$ 	btnDelete��5I      $��1��60   $��
  Delete  ��6@   $��0,0,10,0q�6U   �� 5J      -   5        $ btnSave��5J      $��2��6.   $��  Save  ��6>   6Q   5K      5L      5M      5N      5O      