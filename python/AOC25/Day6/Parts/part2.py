from pathlib import Path

import numpy as np


class Part2:
    def __init__(self, input_file: Path):
        with open(input_file, "r") as f:
            self.input_data = [list(line.replace("\n", "")) for line in f.readlines()]

    def get_day_solution(self):
        total_sum = 0

        # we transpose this mess to make it look like normal math
        input_transposed = np.array(self.input_data).transpose()

        op_mode = ""
        cur_sum = 0
        for line in input_transposed:
            line = "".join(line)

            op = line[-1]
            num = line[:-1].strip()

            if num == "":
                continue
            else:
                num = int(num)

            # we check if we're at a new group/column
            if op in ["+", "*"]:
                total_sum += cur_sum
                cur_sum = 0
                op_mode = op

            # we calculate local group result
            if op_mode == "+":
                cur_sum += num
            else:
                if cur_sum == 0:
                    cur_sum = num
                else:
                    cur_sum *= num

        total_sum += cur_sum

        return str(total_sum)
