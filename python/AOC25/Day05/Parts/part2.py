import re
from pathlib import Path

from Parts.Util.handlers import merge_ranges


class Part2:
    def __init__(self, input_file: Path):
        with open(input_file, "r") as f:
            self.input_data = f.readlines()

        self.id_ranges = []

        for line in self.input_data:
            if match := re.match(r"^(\d+)-(\d+)$", line):
                self.id_ranges.insert(0, [int(match.group(1)), int(match.group(2))])

    def get_day_solution(self):
        fresh_ingredients = 0
        merged_ranges = merge_ranges(self.id_ranges)

        for range in merged_ranges:
            fresh_ingredients += range[1] - range[0] + 1

        return str(fresh_ingredients)
