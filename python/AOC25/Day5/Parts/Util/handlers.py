def merge_ranges(ranges):
    # merge ranges, from lower to higher
    ranges.sort(key=lambda range: range[0])
    merged_ranges = [ranges[0]]

    for range in ranges[1:]:
        last_merged = merged_ranges[0]

        if range[0] <= last_merged[1]:
            # if start of range is in existing merged range, extend it
            last_merged[1] = max(range[1], last_merged[1])
        else:
            # if no overlap, add new range to be merged
            merged_ranges.insert(0, range)

    return merged_ranges
