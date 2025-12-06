import re
from pathlib import Path


class Part1:
    def __init__(self, input_file: Path):
        with open(input_file, "r") as f:
            self.input_data = f.readlines()

    def get_day_solution(self):
        # extract the operators, and group with an accumulator
        accs = [[c, 0] for c in re.findall(r"[+|*]", self.input_data[-1])]

        for line in self.input_data:
            # we find all number groups
            nums = re.findall(r"\d+", line)
            if nums:
                for i, n in enumerate(nums):
                    acc = accs[i]
                    num = int(n)

                    # each group is calculated with its accumulator
                    if acc[0] == "+":
                        acc[1] += num
                    else:
                        if acc[1] == 0:
                            acc[1] = num
                        else:
                            acc[1] *= num

        # finally, accumulators are accumulated
        total_sum = 0
        for _, n in accs:
            total_sum += n

        return str(total_sum)
