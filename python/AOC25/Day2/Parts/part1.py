import itertools
import multiprocessing as mp
import re
from functools import partial
from pathlib import Path

from Parts.Util.handlers import handle_range


def handle_id(id):
    # regex: same number series exactly twice
    match = re.match(r"^(\d+)\1$", str(id))
    if match:
        return id


class Part1:
    def __init__(self, input_file: Path):
        with open(input_file, "r") as f:
            matches: list(str) = re.findall(r"(\d+)-(\d+)", f.readline())

        self.worker_pool = mp.Pool()
        self.id_ranges = [range(int(match[0]), int(match[1]) + 1) for match in matches]

    def get_day_solution(self):
        return str(
            sum(  # summing invalid ids
                itertools.chain(  # chaining resulting lists
                    *self.worker_pool.map(  # ranges are handled in parallel
                        partial(handle_range, handle_id), self.id_ranges
                    )
                )
            )
        )
