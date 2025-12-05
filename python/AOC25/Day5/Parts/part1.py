import re
from pathlib import Path

from Parts.Util.handlers import merge_ranges


class Part1:
    def __init__(self, input_file: Path):
        with open(input_file, "r") as f:
            self.input_data = f.readlines()

        self.id_ranges = []
        self.id_stock = []

        for line in self.input_data:
            if match := re.match(r"^(\d+)-(\d+)$", line):
                self.id_ranges.insert(0, [int(match.group(1)), int(match.group(2))])
            elif match := re.match(r"^\d+$", line):
                self.id_stock.insert(0, int(match.group(0)))

    def get_day_solution(self):
        fresh_ingredients = 0
        merged_ranges = merge_ranges(self.id_ranges)

        for id in self.id_stock:
            for range in merged_ranges:
                if range[0] <= id and id <= range[1]:
                    fresh_ingredients += 1
                    break

        return str(fresh_ingredients)
