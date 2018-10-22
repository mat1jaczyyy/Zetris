﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PPTMonitor {
    class LogicHelper {
        public class Solution : IComparable<Solution> {
            public int desiredX = -1, desiredR = -1;
            public int originalX = -1, originalR = -1;
            public int[,] desiredBoard = new int[10, 40];
            public List<Solution> solutions = new List<Solution>();
            public int? rating = null;

            public void Expand(List<int> queue) {
                if (queue.Count > 0) {
                    foreach (Solution solution in LogicHelper.findAllMoves(desiredBoard, queue[0], originalX, originalR)) {
                        solutions.Add(solution);
                    }

                    List<int> next = queue.ToList();

                    foreach (Solution solution in solutions) {
                        solution.Expand(next.Skip(1).ToList());
                    }

                    rating = solutions.Max().rating;

                } else {
                    rating = rateBoard(desiredBoard);
                }
            }

            public int CompareTo(Solution that) {
                if (this.rating > that.rating) return 1;
                if (this.rating == that.rating) return 0;
                return -1;
            }

            public Solution() {}

            public Solution(int x, int r, int ox, int or, int[,] board) {
                desiredX = x;
                desiredR = r;

                if (ox == -1) {
                    originalX = x;
                } else {
                    originalX = ox;
                }
                if (or == -1) {
                    originalR = r;
                } else {
                    originalR = or;
                }

                desiredBoard = board;
            }
        }

        private static int[][][,] pieces = new int[7][][,] {
            new int[4][,] { // S
                new int[,] {
                    {-1, 0, 0, -1},
                    {0, 0, -1, -1},
                    {-1, -1, -1, -1},
                    {-1, -1, -1, -1}
                },
                new int[,] {
                    {-1, 0, -1, -1},
                    {-1, 0, 0, -1},
                    {-1, -1, 0, -1},
                    {-1, -1, -1, -1}
                },
                new int[,] {
                    {-1, -1, -1, -1},
                    {-1, 0, 0, -1},
                    {0, 0, -1, -1},
                    {-1, -1, -1, -1}
                },
                new int[,] {
                    {0, -1, -1, -1},
                    {0, 0, -1, -1},
                    {-1, 0, -1, -1},
                    {-1, -1, -1, -1}
                }
            },
            new int[4][,] { // Z
                new int[,] {
                    {1, 1, -1, -1},
                    {-1, 1, 1, -1},
                    {-1, -1, -1, -1},
                    {-1, -1, -1, -1}
                },
                new int[,] {
                    {-1, -1, 1, -1},
                    {-1, 1, 1, -1},
                    {-1, 1, -1, -1},
                    {-1, -1, -1, -1}
                },
                new int[,] {
                    {-1, -1, -1, -1},
                    {1, 1, -1, -1},
                    {-1, 1, 1, -1},
                    {-1, -1, -1, -1}
                },
                new int[,] {
                    {-1, 1, -1, -1},
                    {1, 1, -1, -1},
                    {1, -1, -1, -1},
                    {-1, -1, -1, -1}
                }
            },
            new int[4][,] { // J
                new int[,] {
                    {2, -1, -1, -1},
                    {2, 2, 2, -1},
                    {-1, -1, -1, -1},
                    {-1, -1, -1, -1}
                },
                new int[,] {
                    {-1, 2, 2, -1},
                    {-1, 2, -1, -1},
                    {-1, 2, -1, -1},
                    {-1, -1, -1, -1}
                },
                new int[,] {
                    {-1, -1, -1, -1},
                    {2, 2, 2, -1},
                    {-1, -1, 2, -1},
                    {-1, -1, -1, -1}
                },
                new int[,] {
                    {-1, 2, -1, -1},
                    {-1, 2, -1, -1},
                    {2, 2, -1, -1},
                    {-1, -1, -1, -1}
                }
            },
            new int[4][,] { // L
                new int[,] {
                    {-1, -1, 3, -1},
                    {3, 3, 3, -1},
                    {-1, -1, -1, -1},
                    {-1, -1, -1, -1}
                },
                new int[,] {
                    {-1, 3, -1, -1},
                    {-1, 3, -1, -1},
                    {-1, 3, 3, -1},
                    {-1, -1, -1, -1}
                },
                new int[,] {
                    {-1, -1, -1, -1},
                    {3, 3, 3, -1},
                    {3, -1, -1, -1},
                    {-1, -1, -1, -1}
                },
                new int[,] {
                    {3, 3, -1, -1},
                    {-1, 3, -1, -1},
                    {-1, 3, -1, -1},
                    {-1, -1, -1, -1}
                }
            },
            new int[4][,] { // T
                new int[,] {
                    {-1, 4, -1, -1},
                    {4, 4, 4, -1},
                    {-1, -1, -1, -1},
                    {-1, -1, -1, -1}
                },
                new int[,] {
                    {-1, 4, -1, -1},
                    {-1, 4, 4, -1},
                    {-1, 4, -1, -1},
                    {-1, -1, -1, -1}
                },
                new int[,] {
                    {-1, -1, -1, -1},
                    {4, 4, 4, -1},
                    {-1, 4, -1, -1},
                    {-1, -1, -1, -1}
                },
                new int[,] {
                    {-1, 4, -1, -1},
                    {4, 4, -1, -1},
                    {-1, 4, -1, -1},
                    {-1, -1, -1, -1}
                }
            },
            new int[4][,] { // O
                new int[,] { 
                    {-1, 5, 5, -1},
                    {-1, 5, 5, -1},
                    {-1, -1, -1, -1},
                    {-1, -1, -1, -1}
                },
                new int[,] {
                    {-1, 5, 5, -1},
                    {-1, 5, 5, -1},
                    {-1, -1, -1, -1},
                    {-1, -1, -1, -1}
                },
                new int[,] {
                    {-1, 5, 5, -1},
                    {-1, 5, 5, -1},
                    {-1, -1, -1, -1},
                    {-1, -1, -1, -1}
                },
                new int[,] {
                    {-1, 5, 5, -1},
                    {-1, 5, 5, -1},
                    {-1, -1, -1, -1},
                    {-1, -1, -1, -1}
                }
            },
            new int[4][,] { // I
                new int[,] {
                    {-1, -1, -1, -1},
                    {6, 6, 6, 6},
                    {-1, -1, -1, -1},
                    {-1, -1, -1, -1}
                },
                new int[,] {
                    {-1, 6, -1, -1},
                    {-1, 6, -1, -1},
                    {-1, 6, -1, -1},
                    {-1, 6, -1, -1}
                },
                new int[,] {
                    {-1, -1, -1, -1},
                    {-1, -1, -1, -1},
                    {6, 6, 6, 6},
                    {-1, -1, -1, -1}
                },
                new int[,] {
                    {-1, 6, -1, -1},
                    {-1, 6, -1, -1},
                    {-1, 6, -1, -1},
                    {-1, 6, -1, -1}
                }
            }
        };

        private static bool canPlace(int[,] board, int[,] piece, int x, int i, int pi, int pr) {
            for (int j = 0; j < 4; j++) {
                for (int k = 0; k < 4; k++) {
                    if (piece[j, k] != -1) {
                        try {
                            if (board[x + k, i - j] != -1) {
                                return false;
                            }
                        } catch {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private static int[,] applyPiece(int[,] board, int[,] piece, int x, int y, int pi, int pr) {
            int[,] ret = (int[,])board.Clone();

            for (int j = 0; j < 4; j++) {
                for (int k = 0; k < 4; k++) {
                    if (piece[j, k] != -1) {
                        ret[x + k, y - j] = piece[j, k];
                    }
                }
            }

            return ret;
        }

        private static int[,] placePiece(int[,] board, int piece, int x, int r) {
            int i; x--;

            for (i = 39; i >= 0; i--) {
                if (!canPlace(board, pieces[piece][r], x, i, piece, r)) {
                    break;
                }
            }

            if (i > 23) {
                return board;
            }

            return applyPiece(board, pieces[piece][r], x, i + 1, piece, r);
        }

        private static int[] columnHeight(int[,] board) {
            int[] ret = new int[10];
            for (int i = 0; i < 10; i++) {
                for (int j = 39; j >= 0; j--) {
                    if (board[i, j] != -1) {
                        ret[i] = j + 1;
                        break;
                    }
                }
            }
            return ret;
        }

        private static int boardHeight(int[,] board) {
            return columnHeight(board).Sum();
        }

        private static int boardLines(int[,] board) {
            int ret = 0;

            for (int j = 0; j < 40; j++) {
                for (int i = 0; i < 10; i++) {
                    if (board[i, j] == -1) {
                        ret--;
                        break;
                    }
                }
                ret++;
            }

            return ret;
        }

        private static int boardHoles(int[,] board) {
            int ret = 0;

            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 39; j++) {
                    if (board[i, j] == -1 && board[i, j + 1] != -1) {
                        ret++;
                        break;
                    }
                }
            }

            return ret;
        }

        private static int boardBumpiness(int[,] board) {
            int ret = 0;
            int[] height = columnHeight(board);

            for (int i = 0; i < 9; i++) {
                ret += Math.Abs(height[i] - height[i + 1]);
            }

            return ret;
        }

        private static int rateBoard(int[,] board) {
            int height = boardHeight(board);
            int lines = boardLines(board);
            int holes = boardHoles(board);
            int bumpiness = boardBumpiness(board);

            return -102 * height + 152 * lines - 71 * holes - 37 *bumpiness;
        }

        private static List<Solution> findAllMoves(int[,] board, int piece, int ox, int or) {
            List<Solution> ret = new List<Solution>();
            int[,] tempBoard;

            switch (piece) {
                case 0: // S
                case 1: // Z
                    for (int i = 1; i <= 8; i++) {
                        tempBoard = LogicHelper.placePiece(board, piece, i, 0);
                        ret.Add(new Solution(i, 0, ox, or, tempBoard));
                    }
                    for (int i = 4; i <= 8; i++) {
                        tempBoard = LogicHelper.placePiece(board, piece, i, 1);
                        ret.Add(new Solution(i, 1, ox, or, tempBoard));
                    }
                    for (int i = 1; i <= 4; i++) {
                        tempBoard = LogicHelper.placePiece(board, piece, i, 3);
                        ret.Add(new Solution(i, 3, ox, or, tempBoard));
                    }
                    break;

                case 2: // J
                case 3: // L
                case 4: // T
                    for (int i = 1; i <= 8; i++) {
                        tempBoard = LogicHelper.placePiece(board, piece, i, 0);
                        ret.Add(new Solution(i, 0, ox, or, tempBoard));
                    }
                    for (int i = 0; i <= 8; i++) {
                        tempBoard = LogicHelper.placePiece(board, piece, i, 1);
                        ret.Add(new Solution(i, 1, ox, or, tempBoard));
                    }
                    for (int i = 1; i <= 8; i++) {
                        tempBoard = LogicHelper.placePiece(board, piece, i, 2);
                        ret.Add(new Solution(i, 2, ox, or, tempBoard));
                    }
                    for (int i = 1; i <= 9; i++) {
                        tempBoard = LogicHelper.placePiece(board, piece, i, 3);
                        ret.Add(new Solution(i, 3, ox, or, tempBoard));
                    }
                    break;

                case 5: // O
                    for (int i = 0; i <= 8; i++) {
                        tempBoard = LogicHelper.placePiece(board, piece, i, 0);
                        ret.Add(new Solution(i, 0, ox, or, tempBoard));
                    }
                    break;

                case 6: // I
                    for (int i = 1; i <= 7; i++) {
                        tempBoard = LogicHelper.placePiece(board, piece, i, 0);
                        ret.Add(new Solution(i, 0, ox, or, tempBoard));
                    }
                    for (int i = 5; i <= 9; i++) {
                        tempBoard = LogicHelper.placePiece(board, piece, i, 1);
                        ret.Add(new Solution(i, 1, ox, or, tempBoard));
                    }
                    for (int i = 0; i <= 4; i++) {
                        tempBoard = LogicHelper.placePiece(board, piece, i, 3);
                        ret.Add(new Solution(i, 3, ox, or, tempBoard));
                    }
                    break;
            }

            return ret;
        }
        
        public static Solution findMove(int[,] board, int piece, int[] queue) {
            List<Solution> solutions = findAllMoves(board, piece, -1, -1);

            List<int> next = queue.ToList();
            
            foreach (Solution solution in solutions) {
                solution.Expand(next);
            }

            return solutions.Max();
        }
    }
}