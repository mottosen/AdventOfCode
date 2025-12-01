import re
from pathlib import Path


class Part1:
    def __init__(self, input_file: Path):
        with open(input_file, "r") as f:
            self.input_data: list(str) = f.readlines()

        self.max_degree: int = 100
        self.start_angle: int = 50

    def get_day_solution(self):
        cur_angle: int = self.start_angle
        times_zero: int = 0

        for line in self.input_data:
            match = re.search(r"(\w)(\d+)", line)

            if match:
                turn_dir = match.group(1)
                turn_amount = match.group(2)

                if turn_dir == "R":
                    cur_angle += int(turn_amount)
                else:
                    cur_angle -= int(turn_amount)

                cur_angle = cur_angle % self.max_degree

                if cur_angle == 0:
                    times_zero += 1

        return str(times_zero)
