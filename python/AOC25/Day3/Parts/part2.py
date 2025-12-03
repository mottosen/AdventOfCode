import functools as ft
from pathlib import Path

import Parts.Util.handlers as util


class Part2:
    def __init__(self, input_file: Path):
        with open(input_file, "r") as f:
            self.input_data = f.readlines()

        self.batteries = 12

    def get_day_solution(self):
        return str(
            sum(map(ft.partial(util.handle_line, self.batteries), self.input_data))
        )
