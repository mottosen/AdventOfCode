import re
from pathlib import Path


class Part2:
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
                turn_amount = int(match.group(2))

                # we turn multiple rounds
                times_zero += int(turn_amount / 100)
                turn_amount = turn_amount % 100

                if turn_amount > 0:
                    if turn_dir == "R":
                        # cross 0
                        if cur_angle != 0 and turn_amount > self.max_degree - cur_angle:
                            times_zero += 1

                        cur_angle += turn_amount % self.max_degree
                    else:
                        # cross 0
                        if cur_angle != 0 and turn_amount > cur_angle:
                            times_zero += 1

                        cur_angle -= turn_amount % self.max_degree

                    cur_angle = cur_angle % self.max_degree

                    # land on 0
                    if cur_angle == 0:
                        times_zero += 1

        return str(times_zero)
