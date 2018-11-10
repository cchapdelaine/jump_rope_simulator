# -*- coding: utf-8 -*-

import wiiboard

board = wiiboard.Board()

# tuple of weights of each corner
# (AA, BB, CC, DD)
# +----------------+
# |  BB       AA   |
# |  DD       CC   |
# |     Button     |
# +----------------+



# show total weight
print(board.weights.total)



# toggle led of front button
board.toggle_led()